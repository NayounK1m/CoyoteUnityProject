namespace Mapbox.Examples
{
	using UnityEngine;
	using Mapbox.Utils;
	using Mapbox.Unity.Map;
	using Mapbox.Unity.MeshGeneration.Factories;
	using Mapbox.Unity.Utilities;
	using System.Collections.Generic;

	public class SpawnOnMap : MonoBehaviour
	{
		[SerializeField]
		AbstractMap _map;

		[Geocode]
		string[] _locationStrings = new string[3];
		Vector2d[] _locations;

		[Geocode]
		string[] _locationCoyoteStrings = new string[5];
		Vector2d[] _locationsCoyote;

		[SerializeField]
		float _spawnScale = 1f;

		[SerializeField]
		GameObject _markerPrefab;
		[SerializeField]
		GameObject _coyotePrefab;

		List<GameObject> _spawnedObjects;
		List<GameObject> _spawnedCoyoteObjects;

		void Start()
		{
			//센서
			_locations = new Vector2d[_locationStrings.Length];
			_spawnedObjects = new List<GameObject>();
			for (int i = 0; i < _locationStrings.Length; i++)
			{
				var locationString = SingletonLatLng.instance.LatSensor[i] + "," + SingletonLatLng.instance.LngSensor[i];
				_locations[i] = Conversions.StringToLatLon(locationString);
				var instance = Instantiate(_markerPrefab);
				instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
				instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
				_spawnedObjects.Add(instance);
			}

			//코요태
			_locationsCoyote = new Vector2d[_locationCoyoteStrings.Length];
			_spawnedCoyoteObjects = new List<GameObject>();
			for (int i = 0; i < _locationCoyoteStrings.Length; i++)
            {
                var locationString = SingletonLatLng.instance.CoyoteLat[i] + "," + SingletonLatLng.instance.CoyoteLng[i];
				_locationsCoyote[i] = Conversions.StringToLatLon(locationString);
                var instance = Instantiate(_coyotePrefab);
                instance.transform.localPosition = _map.GeoToWorldPosition(_locationsCoyote[i], true);
                instance.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
				_spawnedCoyoteObjects.Add(instance);
            }
        }

		private void Update()
		{
            int count = _spawnedObjects.Count;
            for (int i = 0; i < count; i++)
            {
                var spawnedObject = _spawnedObjects[i];
                var location = _locations[i];
                spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
                spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
            }

			int countCoyote = _spawnedCoyoteObjects.Count;
			for (int i = 0; i < countCoyote; i++)
			{
				var spawnedCoyoteObject = _spawnedCoyoteObjects[i];
				var locationCoyote = _locationsCoyote[i];
				spawnedCoyoteObject.transform.localPosition = _map.GeoToWorldPosition(locationCoyote, true);
				spawnedCoyoteObject.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
			}
		}
	}
}