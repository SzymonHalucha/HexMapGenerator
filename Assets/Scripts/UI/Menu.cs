using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HexMapGenerator.Generation;
using HexMapGenerator.Utils;

namespace HexMapGenerator.UI
{
    public class Menu : MonoBehaviour
    {
        public GeneratorData Generator64;
        public GeneratorData Generator128;

        private Map _map;
        private TMP_InputField _seedInput;
        private int _currentSeed = 0;

        private void Awake()
        {
            _map = FindObjectOfType<Map>();
            _seedInput = this.transform.GetComponentInChildren<TMP_InputField>();
            _seedInput.DeactivateInputField();
        }

        public void OnSeedInputChanged(string value)
        {
            _currentSeed = Int32.Parse(value);
        }

        public void OnGenerate64()
        {
            if (_currentSeed == Generator64.Seed) ChangeSeedToRandom();

            Generator64.Initialize(_currentSeed);
            _map.GenerateMap(Generator64);
        }

        public void OnGenerate128()
        {
            if (_currentSeed == Generator128.Seed) ChangeSeedToRandom();

            Generator128.Initialize(_currentSeed);
            _map.GenerateMap(Generator128);
        }

        public void OnExit()
        {
            Application.Quit();
        }

        private void ChangeSeedToRandom()
        {
            _currentSeed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
            _seedInput.text = _currentSeed.ToString();
            _seedInput.DeactivateInputField();
        }
    }
}