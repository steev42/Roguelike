using System;
using UnityEngine;

public class WuLineAlgorithm
{
    public WuLineAlgorithm()
    {
    }

    void plot(int x, int y, float c)
    {
        //plot the pixel at (x, y) with brightness c (where 0 ≤ c ≤ 1)
    }

    // integer part of x
    int ipart(float x)
    {
        return (int)x;
    }

    float round(float x)
    {
        return ipart(x + 0.5f);
    }

    void swap(float a, float b)
    {
        float t = a;
        a = b;
        b = t;
    }

    // fractional part of x
    float fpart(float x)
    {
        if (x < 0)
            return 1 - (x - Mathf.Floor(x));
        return x - Mathf.Floor(x);
    }

    float rfpart(float x)
    {
        return 1 - fpart(x);
    }

    void drawLine(float x0, float y0, float x1, float y1)
    {

        bool steep = Mathf.Abs(y1 - y0) > Mathf.Abs(x1 - x0);

        if (steep)
        {
            swap(x0, y0);
            swap(x1, y1);
        }
        if (x0 > x1)
        { 
            swap(x0, x1);
            swap(y0, y1);
        }

        float dx = x1 - x0;
        float dy = y1 - y0;
        float gradient = dy / dx;
        if (dx == 0.0)
        {
            gradient = 1.0f;
        }

        // handle first endpoint
        float xend = round(x0);
        float yend = y0 + gradient * (xend - x0);
        float xgap = rfpart(x0 + 0.5f);
        float xpxl1 = xend;// this will be used in the main loop
        float ypxl1 = ipart(yend);
        if (steep)
        {
            plot((int)ypxl1, (int)xpxl1, rfpart(yend) * xgap);
            plot((int)ypxl1 + 1, (int)xpxl1, fpart(yend) * xgap);
        }
        else
        {
            plot((int)xpxl1, (int)ypxl1, rfpart(yend) * xgap);
            plot((int)xpxl1, (int)ypxl1 + 1, fpart(yend) * xgap);
        }
        float intery = yend + gradient;// first y-intersection for the main loop

        // handle second endpoint
        xend = round(x1);
        yend = y1 + gradient * (xend - x1);
        xgap = fpart(x1 + 0.5f);
        float xpxl2 = xend;//this will be used in the main loop
        float ypxl2 = ipart(yend);
        if (steep)
        {
            plot((int)ypxl2, (int)xpxl2, rfpart(yend) * xgap);
            plot((int)ypxl2 + 1, (int)xpxl2, fpart(yend) * xgap);
        }
        else
        {
            plot((int)xpxl2, (int)ypxl2, rfpart(yend) * xgap);
            plot((int)xpxl2, (int)ypxl2 + 1, fpart(yend) * xgap);
        }
        // main loop
        if (steep)
        {
            for (float x = xpxl1 + 1f; x <= xpxl2 - 1f; x++)
            {
                                        
                plot(ipart(intery), (int)x, rfpart(intery));
                plot(ipart(intery) + 1, (int)x, fpart(intery));
                intery = intery + gradient;
            }
        }
        else
        {
            for (float x = xpxl1 + 1f; x <= xpxl2 - 1f; x++)
            {
                plot((int)x, ipart(intery), rfpart(intery));
                plot((int)x, ipart(intery) + 1, fpart(intery));
                intery = intery + gradient;
            }
        }
    }
}


