using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class HealthDataStore
{
    private static Dictionary<string, HealthDataStore> healthDataStoreForAll = new Dictionary<string, HealthDataStore>();

    private int _currentHealth;
    private bool _isPlayerDead;
    private int _minHealth = 1;
    private int _maxHealth = 10;
    private int _defaultHealthDamageWorth = 2;
    private int _defaultHealthRestoreWorth = 2;

    public static HealthDataStore getInstance(string type)
    {
        if (healthDataStoreForAll.ContainsKey(type))
        {
            return healthDataStoreForAll[type];
        }

        healthDataStoreForAll[type] = new HealthDataStore();
        healthDataStoreForAll[type].init();

        return healthDataStoreForAll[type];
    }

    public static HealthDataStore getOtherPlayerInstance(string currentPlayerType)
    {
        string otherPlayerKey = null;

        foreach (string key in healthDataStoreForAll.Keys) 
        { 
            if (key.Equals(currentPlayerType))
            {
                continue;
            }

            otherPlayerKey = key;
            break;
        }

        if (otherPlayerKey == null)
        {
            throw new System.Exception("Other player not found");
        }

        return healthDataStoreForAll[otherPlayerKey];
    }

    private void init()
    {
        this._currentHealth = this._maxHealth;
        this._isPlayerDead = false;
    }

    public HealthDataStore increaseHealth()
    {
        int newHealth = this._currentHealth + this._defaultHealthRestoreWorth;

        if (newHealth > _maxHealth)
        {
            this._currentHealth = _maxHealth;
            return this;
        }

        this._currentHealth = newHealth;
        return this;
    }

    public HealthDataStore reduceHealth()
    {
        int newHealth = this._currentHealth - _defaultHealthDamageWorth;

        if (newHealth < _minHealth)
        {
            SceneManager.LoadScene("GameOver");
            this._currentHealth = 0;
            this._isPlayerDead = true;
            return this;
        }

        this._currentHealth = newHealth;
        return this;
    }

    public int getCurrentHealth()
    {
        return this._currentHealth;
    }

    public int getMaxHealth()
    {
        return this._maxHealth;
    }

    public bool isPlayerDead()
    {
        return _isPlayerDead;
    }

    public HealthDataStore reset()
    {
        this.init();
        return this;
    }

    public static void resetAll()
    {
        healthDataStoreForAll.Clear();
    }

    public Color getHealthColor()
    {
        switch (this._currentHealth)
        {
            case 10: // Full Health
                return Color.green; // Bright, vibrant green
            case 8: // 80% Health
                return new Color(0.5f, 1f, 0.5f); // Lighter green
            case 6: // 60% Health
                return Color.yellow; // Warning yellow
            case 4: // 40% Health
                return new Color(1f, 0.5f, 0f); // Orange
            case 2: // 20% Health
                return Color.red; // Critical red
            case 0: // No Health
                return new Color(0.3f, 0f, 0f); // Dark, deep red/maroon
            default:
                return Color.gray; // Fallback color
        }
    }

    public Color getHealthColorNewerApproach()
    {
        float healthPercentage = this._currentHealth / this._maxHealth;

        // Create a smooth color gradient from green to red
        Color smoothColor = Color.Lerp(Color.red, Color.green, healthPercentage);

        // Optional: Adjust saturation and brightness for more visual appeal
        return Color.Lerp(smoothColor, Color.white, 0.3f);
    }
}