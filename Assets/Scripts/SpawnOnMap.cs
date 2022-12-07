/* 
 * // Spawn On Map (Mapping) //
 * It is a code that generates pins to be mapped on a map and updates the mapped pins according to the state and movement of the map.
 * Lat means latitude, Lng means longitude.
 */

namespace Mapbox.Examples
{
	using UnityEngine;
	using Mapbox.Utils;
	using Mapbox.Unity.Map;
	using Mapbox.Unity.Utilities;
	using System.Collections.Generic;

	public class SpawnOnMap : MonoBehaviour
	{
		[SerializeField]
		AbstractMap _map; //Map prefab

		[SerializeField]
		Camera mainCamera;

		//Position value of sensor
		[Geocode]
		string[] _locationStrings = new string[3];
		Vector2d[] _locations;

		//Position value of coyote
		[Geocode]
		Vector2d[] _locationsCoyote;

		[SerializeField]
		float _spawnScale = 1f;

		//Sensor Prefab(Model) Pin
		[SerializeField]
		GameObject _markerPrefab;
		//Coyote Prefab(Model) Pin
		[SerializeField]
		GameObject _coyotePrefab;
		//Most recently detected Coyote prefab(Model) Pin
		[SerializeField]
		GameObject _newCoyotePrefab;

		//List of models that have already been spawn
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
			_locationsCoyote = new Vector2d[SingletonLatLng.instance.CoyoteLat.Count];
			_spawnedCoyoteObjects = new List<GameObject>();
			for (int i = 0; i < SingletonLatLng.instance.CoyoteLat.Count; i++)
            {
				var locationString = SingletonLatLng.instance.CoyoteLat[i] + "," + SingletonLatLng.instance.CoyoteLng[i];
				_locationsCoyote[i] = Conversions.StringToLatLon(locationString);
                var instance = Instantiate(_coyotePrefab);
				instance.transform.name = "CoyotePin" + i;
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
			//여태까지 소환된 모델 수 만큼 반복
			for (int i = 0; i < countCoyote; i++)
			{
				//여태까지 소환된 모델들 다 월드 포지션 --> 로컬 포지션 변환, 월드 사이즈 --> 로컬 사이즈 변환
				var spawnedCoyoteObject = _spawnedCoyoteObjects[i];
				Vector2d[] _locationsCoyoteUpdate = new Vector2d[SingletonLatLng.instance.CoyoteLat.Count];
				var locationString = SingletonLatLng.instance.CoyoteLat[i] + "," + SingletonLatLng.instance.CoyoteLng[i];
				_locationsCoyoteUpdate[i] = Conversions.StringToLatLon(locationString);
				var locationCoyote = _locationsCoyoteUpdate[i];
				spawnedCoyoteObject.transform.localPosition = _map.GeoToWorldPosition(locationCoyote, true);
				spawnedCoyoteObject.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
			}

			//실시간 코요태 추가
			if (_spawnedCoyoteObjects.Count < SingletonLatLng.instance.CoyoteLat.Count)
			{
				//추가되는 코드
				var locationString = SingletonLatLng.instance.CoyoteLat[SingletonLatLng.instance.CoyoteLat.Count -1] + "," + SingletonLatLng.instance.CoyoteLng[SingletonLatLng.instance.CoyoteLat.Count - 1];
				var instance = Instantiate(_newCoyotePrefab); //_locationsCoyote[index]
				instance.transform.localPosition = _map.GeoToWorldPosition(Conversions.StringToLatLon(locationString), true);
				instance.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
				instance.transform.name = "CoyotePin" + _spawnedCoyoteObjects.Count;
				mainCamera.transform.localPosition = new Vector3(instance.transform.localPosition.x, 30, instance.transform.localPosition.z);
				_spawnedCoyoteObjects.Add(instance);
			}
		}
	}
}