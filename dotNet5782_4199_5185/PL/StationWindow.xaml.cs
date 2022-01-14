﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BO;
using BlApi;
using System.Collections.ObjectModel;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationWindow.xaml
    /// </summary>
    public partial class StationWindow : Window
    {
        BaseStation baseStation = new BaseStation();// insert the input of user to this object
        ObservableCollection<DroneInCharging> ListOfInCharge = new ObservableCollection<DroneInCharging>();
        private IBL BLAccess;
        public StationWindow(IBL BLAccess)
        {
            this.BLAccess = BLAccess;
            InitializeComponent();
            btnUpdateChargeSlots.Visibility = Visibility.Hidden;
            btnUpdateName.Visibility = Visibility.Hidden;
        }

        public StationWindow(IBL BLAccess, BaseStation BaseStation)
        {

            InitializeComponent();
            baseStation = BaseStation;
          
            DroneINchargeListView.DataContext = baseStation.DroneINCharge;
            
            DataContext = baseStation;
            txtName.Text = baseStation.Name.ToString();
            IDfill.Text = baseStation.Id.ToString();
            AvailableSlots.Text = baseStation.NumberOfavailableChargingSlots.ToString();
            Location_longi.Text = baseStation.Location.Longitude.ToString();
            Location_lati.Text = baseStation.Location.Latitude.ToString();
            

            add_station.Visibility = Visibility.Hidden;








        }



        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void add_Station_Click(object sender, RoutedEventArgs e)
        {

            bool flag = true;
            baseStation.DroneINCharge = null;
            baseStation.Id = Convert.ToInt32(IDfill.Text);
            baseStation.Name = txtName.Text;
            baseStation.NumberOfavailableChargingSlots = int.Parse(AvailableSlots.Text);
            baseStation.Location = new(Convert.ToDouble(Location_longi.Text), Convert.ToDouble(Location_lati.Text));


            try
            {
                BLAccess.AddStation(baseStation);
                MessageBox.Show("the station was successfully added");
            }
            catch (AlreadyExistsException ex)//check why the massage in not shown
            {
                MessageBox.Show(ex.ToString());
                flag = false;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                flag = false;
            }
            if (!flag)
            {
                var bc = new BrushConverter();
                IDfill.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");

            }
            if (flag)
            {
                this.Close();
            }
        }

        private void DroneINchargeListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void IDfill_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
