
using UnityEngine;
using System;

public static class GPSCoordinateConverter
{
    private const double MetersPerDegree = 111320.0;

    public static Vector2 ToLocalMeters(
        double originLatitude,
        double originLongitude,
        double currentLatitude,
        double currentLongitude)
    {
        // Differenz zwischen Start- und aktueller Position
        double latitudeDifference =
            currentLatitude - originLatitude;

        double longitudeDifference =
            currentLongitude - originLongitude;

        // Umrechnung in Meter
        double northSouth =
            latitudeDifference * MetersPerDegree;

        double eastWest =
            longitudeDifference *
            MetersPerDegree *
            Math.Cos(originLatitude * Math.PI / 180.0);

        // Unity-Koordinaten zurückgeben
        return new Vector2(
            (float)eastWest,
            (float)northSouth
        );
    }
}

