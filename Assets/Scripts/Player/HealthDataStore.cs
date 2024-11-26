using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        return healthDataStoreForAll[type];
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
}