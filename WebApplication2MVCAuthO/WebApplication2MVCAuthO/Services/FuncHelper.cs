using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2MVCAuthO.Services
{
    public class FuncHelper
    {
        public static double Distance(string latitude1, string longitude1, string latitude2, string longitude2)
        {
            double dist = 100D;
            var isLat1 = double.TryParse(latitude1, out var lat1);
            var isLng1 = double.TryParse(longitude1, out var lng1);
            var isLat2 = double.TryParse(latitude2, out var lat2);
            var isLng2 = double.TryParse(longitude2, out var lng2);

            if (isLat1 && isLng1 && isLat2 && isLng2)
            {
                dist = Distance(lat1, lng1, lat2, lng2, 2);
            }

            return dist;
        }

        public static string DistanceFormatted(double distance)
        {
            string dist = "";
            
            if (distance < 100D)
            {
                var arrDist = distance.ToString("##.000").Split(".");
                if (arrDist.Length == 2)
                {
                    if (arrDist[0] == "") arrDist[0] = "0";
                    dist = $"{arrDist[0]}км {arrDist[1]}м";
                }
            }

            return dist;
        }

        public static double Distance(double Latitude1, double Longitude1, double Latitude2, double Longitude2, int type)
        {
            //1- miles
            double R = (type == 1) ? 3960 : 6371;          // R is earth radius.
            double dLat = ToRadian(Latitude2 - Latitude1);
            double dLon = ToRadian(Longitude2 - Longitude1);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(ToRadian(Latitude1)) * Math.Cos(ToRadian(Latitude2)) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = R * c;

            return Math.Round(d, 3);
        }

        private static double ToRadian(double val)
        {
            return (Math.PI / 180) * val;
        }

        private static double distance(double lat1, double lon1, double lat2, double lon2, char unit)
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = dist * 60 * 1.1515;
            if (unit == 'K')
            {
                dist = dist * 1.609344;
            }
            else if (unit == 'N')
            {
                dist = dist * 0.8684;
            }
            return (dist);
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts decimal degrees to radians             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private static double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts radians to decimal degrees             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private static double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }
    }
}
