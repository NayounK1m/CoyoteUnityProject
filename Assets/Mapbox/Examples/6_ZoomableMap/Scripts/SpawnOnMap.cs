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
		//센서 변수
		string[] _locationStrings = new string[3];
		Vector2d[] _locations;

		[Geocode]
		//코요태 변수
		string[] _coyoteLocationStrings = new string[SingletonLatLng.instance.CoyoteLng.Length];
		Vector2d[] _coyoteLocations;

		[SerializeField]
		float _spawnScale = 1f;

		[SerializeField]
		GameObject _markerPrefab;
		GameObject _coyotePrefab;

		List<GameObject> _spawnedObjects;

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
			_coyoteLocations = new Vector2d[_coyoteLocationStrings.Length];
			for (int i = 0; i < _locationStrings.Length; i++)
			{
				var locationString = SingletonLatLng.instance.CoyoteLat[i] + "," + SingletonLatLng.instance.CoyoteLng[i];
				_locations[i] = Conversions.StringToLatLon(locationString);
				var instance = Instantiate(_coyotePrefab);
				instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
				instance.transform.localScale = new Vector3(1, 1, 1);
				_spawnedObjects.Add(instance);
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
		}
	}
}