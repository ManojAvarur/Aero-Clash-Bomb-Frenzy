using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidthAndHeightController
{
    private static WidthAndHeightController instance;

    private float width = .1f;
    private float height = .1f;

    private bool widthInstantiated = false;
    private bool heightInstantiated = false;

    public static WidthAndHeightController getInstance()
    {
        if(instance == null)
        {
            instance = new WidthAndHeightController();
        }

        return instance;
    }

    public WidthAndHeightController changeWidth(float width)
    {
        this.width = width;
        return this;
    }

    public WidthAndHeightController changeHeight(float height)
    {
        this.height = height;
        return this;
    }

    public float getWidth() 
    { 
        return width; 
    }

    public float getHeight() 
    { 
        return height; 
    }

    public WidthAndHeightController setWidthInstantiatedToTrue()
    {
        this.widthInstantiated = true;
        return this;
    }

    public WidthAndHeightController setHeightInstantiatedToTrue()
    {
        this.heightInstantiated = true;
        return this;
    }

    public WidthAndHeightController setWidthInstantiatedToFalse()
    {
        this.widthInstantiated = false;
        return this;
    }

    public WidthAndHeightController setHeightInstantiatedToFalse()
    {
        this.heightInstantiated = false;
        return this;
    }

    public bool isWidthInstantiated() 
    {  
        return widthInstantiated; 
    }

    public bool isHeightInstantiated()
    {
        return heightInstantiated;
    }
}
