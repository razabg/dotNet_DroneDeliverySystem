﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BO;

namespace BL
{
    internal sealed partial class BLObject
    {
        public DO.Customer GetCustomerDetails(int id)
        {
            List<DO.Customer> CustomerListDal = AccessToDataMethods.ReturnCustomerList().ToList();
            int index = CustomerListDal.FindIndex(x => x.Id == id);
            if (index == -1)
            {
                throw new NotExistsException();
            }
            return CustomerListDal[index];
        }

        public double CalcDistanceBetweenTwoCoordinates(double longi1, double lati1, double longi2, double lati2)
        {
            double distance = Math.Sqrt(Math.Pow(longi1 - longi2, 2) + Math.Pow(lati1 - lati2, 2));
            return distance;


        }
        /// <summary>
        /// the method check the parcel status according to its time;
        /// </summary>
        /// <param name="parcel"></param>
        /// <returns></returns>
        public string parcelStatus(DO.Parcel parcel)
        {
            if (parcel.ArrivedTime != null)
            {
                return statusEnum.ParcelStatus.Arrived.ToString();
            }
            else if (parcel.PickedUp != null)
            {
                return statusEnum.ParcelStatus.PickedUp.ToString();
            }
            else if (parcel.ParingTime != null)
            {
                return statusEnum.ParcelStatus.Paired.ToString();
            }

            return statusEnum.ParcelStatus.created.ToString();



        }

        public DO.Station NearestStation(double longi, double lati, IEnumerable<DO.Station> station)
        {
            double MinDist = CalcDistanceBetweenTwoCoordinates(longi, lati, station.First().Longitude, station.First().Latitude);
            DO.Station StationToReturn = station.First();
            foreach (var item in station)
            {
                double temp = CalcDistanceBetweenTwoCoordinates(longi, lati, item.Longitude, item.Latitude);
                if (MinDist > temp)
                {
                    MinDist = temp;
                    StationToReturn = item;
                }
            }
            return StationToReturn;

        }

        public DO.Station NearestStationToChargeDrone(double longi, double lati, IEnumerable<DO.Station> station)
        {

            double MinDist = CalcDistanceBetweenTwoCoordinates(longi, lati, station.First().Longitude, station.First().Latitude);
            bool flag = true;
            List<DO.Station> StationToskip = new List<DO.Station>();
            DO.Station StationToReturn = station.First();
            while (flag == true) //iterate on the stations list until we will find the nearest station that has free charge slot
            {
                foreach (var item in station)
                {
                    double temp = CalcDistanceBetweenTwoCoordinates(longi, lati, item.Longitude, item.Latitude);
                    if (MinDist > temp)
                    {
                        if (!StationToskip.Contains(item)) //it the specific item is on the "black" list which contains the full stations skip to the next station
                        {
                            MinDist = temp;
                            StationToReturn = item;
                        }

                    }
                }
                int numOfDronesInCharge = AccessToDataMethods.ReturnDroneChargeList().Count(x => x.StationId == StationToReturn.Id);// num of the drones that getting charged in the station
                if (numOfDronesInCharge < StationToReturn.ChargeSlots) // if the station has room for another drone
                {
                    return StationToReturn;
                    //flag = false; //stop the outer while loop
                }
                else if (numOfDronesInCharge >= StationToReturn.ChargeSlots)  // if there is no room in the station add the station to the "black list
                {
                    StationToskip.Add(StationToReturn);
                    // flag = true;
                }
                else if (StationToskip.Equals(station))
                {
                    throw new NoStationsWithFreeChargeException();
                }

            }
            return StationToReturn;
        }

        public DO.Customer NearestParcel_SenderIdcustomer(double longi, double lati, IEnumerable<DO.Parcel> parceList)
        {
            List<DO.Customer> SenderOfTheParcel = new List<DO.Customer>();//this list going to contain the customers so that we will know the location of the parcels
            List<DO.Parcel> helpList = parceList.ToList();//help us in the end to extract the closest parcel

            foreach (var parcel in parceList)
            {
                foreach (var customer in AccessToDataMethods.ReturnCustomerList())
                {
                    if (parcel.SenderId == customer.Id)
                    {
                        SenderOfTheParcel.Add(customer);//build a new list of customers that contained the locations of the parcel list in the parameters;
                    }//with this list can calc and finf the closest parcel to our drone

                }

            }

            double MinDist = CalcDistanceBetweenTwoCoordinates(longi, lati, SenderOfTheParcel.First().Longitude, SenderOfTheParcel.First().Latitude);
            DO.Customer customerToReturn = SenderOfTheParcel.First();
            foreach (var item in SenderOfTheParcel)
            {
                double temp = CalcDistanceBetweenTwoCoordinates(longi, lati, item.Longitude, item.Latitude);
                if (MinDist > temp)
                {
                    MinDist = temp;
                    customerToReturn = item;
                }
            }


            var parcelToReturn = helpList.Find(x => x.SenderId == customerToReturn.Id);//we find parcel that hold the sender-customer id which he is the closest parcel to the drone

            //return parcelToReturn;
            return customerToReturn;


        }

        public DO.Customer returnTargetCustomer(DO.Parcel parcel)
        {
            return AccessToDataMethods.ReturnCustomerList().ToList().Find(x => x.Id == parcel.TargetId);
        }


        /// <summary>
        /// A function that converts from decimal to Sexagesimal
        /// </summary>
        /// <param name="decimalValueToConvert">The number to convert</param>
        /// <param name="side"></param>
        /// <returns>string that hold the convert location</returns>
        public static string ConvertDecimalDegreesToSexagesimal(double decimalValueToConvert, string LongOrLat)
        {
            string direction = null;
            if (LongOrLat == "Longitude")
            {
                if (decimalValueToConvert >= 0)
                    direction = "N";
                else direction = "S";
            }

            if (LongOrLat == "Latitude")
            {
                if (decimalValueToConvert >= 0)
                    direction = "E";
                else direction = "W";
            }
            int sec = (int)Math.Round(decimalValueToConvert * 3600);
            int deg = sec / 3600;
            sec = Math.Abs(sec % 3600);
            int min = sec / 60;
            sec %= 60;

            return string.Format("{0}° {1}' {2}'' {3}", Math.Abs(deg), Math.Abs(min), Math.Abs(sec), direction);// return the complited number
        }


      public  int POWERindex(DroneToList check)
        {
            if (check.Weight == statusEnum.TopWeight.Light.ToString())
            {
                return 1;
            }
            if (check.Weight == statusEnum.TopWeight.Average.ToString())
            {
                return 2;
            }
            if (check.Weight == statusEnum.TopWeight.Heavy.ToString())
            {
                return 3;
            }
            return 0;

        }



    }

}