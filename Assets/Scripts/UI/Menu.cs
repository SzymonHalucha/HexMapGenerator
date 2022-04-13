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
    /// <summary>
    /// This class is responsible for handling the UI.
    /// </summary>
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

        /// <summary>
        /// Reads the InputField when the user makes a change to it.
        /// </summary>
        /// <param name="value">Changed value.</param>
        public void OnSeedInputChanged(string value)
        {
            _currentSeed = Int32.Parse(value);
        }

        /// <summary>
        /// Generates a map of size 64x64 hexagons.
        /// </summary>
        public void OnGenerate64()
        {
            if (_currentSeed == Generator64.Seed) ChangeSeedToRandom();

            Generator64.Initialize(_currentSeed);
            _map.GenerateMap(Generator64);
        }

        /// <summary>
        /// Generates a map of 128x128 hexagons.
        /// </summary>
        public void OnGenerate128()
        {
            if (_currentSeed == Generator128.Seed) ChangeSeedToRandom();

            Generator128.Initialize(_currentSeed);
            _map.GenerateMap(Generator128);
        }

        /// <summary>
        /// Leaves application.
        /// </summary>
        public void OnExit()
        {
            Application.Quit();
        }

        /// <summary>
        /// Randomizes the seed for the map generator.
        /// </summary>
        private void ChangeSeedToRandom()
        {
            _currentSeed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
            _seedInput.text = _currentSeed.ToString();
            _seedInput.DeactivateInputField();
        }
    }
}