
using UnityEngine;
using System.Collections;

public class GPSGameController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GPSLocationController gpsController;
    [SerializeField] private Transform playerMarker;
    [SerializeField] private Transform startRadius;

    [Header("Game Settings")]
    [SerializeField] private float radiusInMeters = 5f;

    // Ursprünglicher GPS-Standort
    private double startLatitude;
    private double startLongitude;

    // Tatsächliche Spielerbewegung in Metern
    public Vector2 PlayerMovement { get; private set; }

    // Entfernung zum ursprünglichen Standort
    public float DistanceFromStart { get; private set; }
    public float RadiusInMeters => radiusInMeters;


    public bool IsInitialized { get; private set; }
    public bool HasWon { get; private set; }

    private IEnumerator Start()
    {
        // Auf die GPS-Initialisierung warten
        while (gpsController == null || !gpsController.IsReady)
        {
            yield return null;
        }

        // Ursprünglichen Standort speichern
        startLatitude = gpsController.Latitude;
        startLongitude = gpsController.Longitude;

        // Spieler bleibt immer in der Bildschirmmitte
        playerMarker.position = new Vector3(0f, 0f, playerMarker.position.z);

        // Startkreis zunächst im Zentrum platzieren
        startRadius.position = new Vector3(
            0f,
            0f,
            startRadius.position.z
        );
        
        // Größe des Startkreises anhand des Radius festlegen
        startRadius.localScale = new Vector3(
			radiusInMeters * 2f,
			radiusInMeters * 2f,
			startRadius.localScale.z
		);

        IsInitialized = true;

        Debug.Log("GPS Game initialized!");
        Debug.Log("Start position: " +
                  startLatitude + ", " + startLongitude);
    }

    private void Update()
    {
        if (!IsInitialized || !gpsController.IsReady)
            return;

        // GPS-Koordinaten in lokale Meter umrechnen
        PlayerMovement = GPSCoordinateConverter.ToLocalMeters(
            startLatitude,
            startLongitude,
            gpsController.Latitude,
            gpsController.Longitude
        );

        // Entfernung zum Startpunkt berechnen
        DistanceFromStart = PlayerMovement.magnitude;
        
        // Prüfen, ob der Spieler den Startbereich verlassen hat
		if (!HasWon && DistanceFromStart > radiusInMeters)
		{
			HasWon = true;

			Debug.Log("YOU WON!");
		}

        // Startkreis entgegen der Spielerbewegung verschieben
        Vector2 circlePosition = -PlayerMovement;

        startRadius.position = new Vector3(
            circlePosition.x,
            circlePosition.y,
            startRadius.position.z
        );
    }
}

