
using UnityEngine;
using TMPro;

public class GPSInfoUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GPSLocationController gpsController;
    [SerializeField] private TMP_Text gpsInfoText;
    
    [Header("Game References")]
	[SerializeField] private GPSGameController gameController;


	private void Update()
	{
		if (gpsController == null || gpsInfoText == null)
		    return;

		if (!gpsController.IsReady)
		{
		    gpsInfoText.text =
		        "GPS LOCATION GAME\n\n" +
		        "Status: " + gpsController.Status;

		    return;
		}

		string gameInfo = "";


		if (gameController != null && gameController.IsInitialized)
		{
			gameInfo =
				"\n\n--- GAME ---\n\n" +
				"Distance from start: " +
				gameController.DistanceFromStart.ToString("F2") + " m\n" +
				"Start radius: " +
				gameController.RadiusInMeters.ToString("F2") + " m";

			if (gameController.HasWon)
			{
				gameInfo +=
				    "\n\n*** YOU WON! ***\n" +
				    "Sie haben gewonnen!";
			}
		}


		gpsInfoText.text =
		    "GPS LOCATION GAME\n\n" +
		    "Status: " + gpsController.Status + "\n\n" +
		    "Latitude: " + gpsController.Latitude.ToString("F6") + "\n" +
		    "Longitude: " + gpsController.Longitude.ToString("F6") + "\n" +
		    "Altitude: " + gpsController.Altitude.ToString("F1") + " m\n" +
		    "Accuracy: " + gpsController.HorizontalAccuracy.ToString("F1") + " m" +
		    gameInfo;
	}
}

