
using UnityEngine;
using System.Collections;

public class GPSLocationController : MonoBehaviour
{
    [Header("GPS Settings")]
    [SerializeField] private float desiredAccuracy = 0.01f;
    [SerializeField] private float updateDistance = 0f;

    // Aktuelle Standortdaten
    public float Latitude { get; private set; }
    public float Longitude { get; private set; }
    public float Altitude { get; private set; }
    public float HorizontalAccuracy { get; private set; }

    // Status des GPS-Dienstes
    public bool IsReady { get; private set; }
    public string Status { get; private set; } = "Initializing...";

    private IEnumerator Start()
    {
        // Prüfen, ob Standortdienste verfügbar sind
        if (!Input.location.isEnabledByUser)
        {
            Status = "Location services disabled";
            Debug.LogWarning(Status);
            yield break;
        }

        Status = "Starting GPS...";

        // GPS-Dienst starten
        Input.location.Start(desiredAccuracy, updateDistance);

        // Auf die erste Standortbestimmung warten
        int maxWait = 20;

        while (Input.location.status == LocationServiceStatus.Initializing
               && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Initialisierung fehlgeschlagen?
        if (maxWait <= 0 ||
            Input.location.status == LocationServiceStatus.Failed)
        {
            Status = "GPS initialization failed";
            Debug.LogError(Status);
            yield break;
        }

        // GPS erfolgreich initialisiert
        IsReady = true;
        Status = "GPS active";

        Debug.Log("GPS successfully initialized!");

        // Standortdaten kontinuierlich aktualisieren
        while (true)
        {
            UpdateLocationData();
            yield return new WaitForSeconds(1);
        }
    }

    private void UpdateLocationData()
    {
        if (Input.location.status != LocationServiceStatus.Running)
        {
            IsReady = false;
            Status = "GPS unavailable";
            return;
        }

        LocationInfo location = Input.location.lastData;

        Latitude = location.latitude;
        Longitude = location.longitude;
        Altitude = location.altitude;
        HorizontalAccuracy = location.horizontalAccuracy;
    }

    private void OnDestroy()
    {
        // GPS-Dienst wieder beenden
        Input.location.Stop();
    }
}

