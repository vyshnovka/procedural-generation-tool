using System;

public class DiamondSquare : Algorithm
{
    public override float[,] GenerateHeightMap(int size)
    {
        float[,] noise = new float[size + 1, size + 1];

        noise[0, 0] = noise[0, size] = noise[size, 0] = noise[size, size] = 0;

        Random r = new Random(); //for the new value in range of h
                                 //side length is distance of a single square side
                                 //or distance of diagonal in diamond
        float h = 1f;
        for (int sideLength = size;
            //side length must be >= 2 so we always have
            //a new value (if its 1 we overwrite existing values
            //on the last iteration)
            sideLength >= 2;
            //each iteration we are looking at smaller squares
            //diamonds, and we decrease the variation of the offset
            sideLength /= 2, h /= 2.0f)
        {
            //half the length of the side of a square
            //or distance from diamond center to one corner
            //(just to make calcs below a little clearer)
            int halfSide = sideLength / 2;

            //generate the new square values
            for (int x = 0; x < size; x += sideLength)
            {
                for (int y = 0; y < size; y += sideLength)
                {
                    //x, y is upper left corner of square
                    //calculate average of existing corners
                    float avg = noise[x, y] + //top left
                    noise[x + sideLength, y] +//top right
                    noise[x, y + sideLength] + //lower left
                    noise[x + sideLength, y + sideLength];//lower right
                    avg /= 4.0f;

                    //center is average plus random offset
                    noise[x + halfSide, y + halfSide] = avg + ((float)r.NextDouble() * 2f * h) - h;
                    //We calculate random value in range of 2h
                    //and then subtract h so the end value is
                    //in the range (-h, +h)
                }
            }
            //generate the diamond values
            //since the diamonds are staggered we only move x
            //by half side
            //NOTE: if the data shouldn't wrap then x < DATA_SIZE
            //to generate the far edge values
            for (int x = 0; x < size; x += halfSide)
            {
                //and y is x offset by half a side, but moved by
                //the full side length
                //NOTE: if the data shouldn't wrap then y < DATA_SIZE
                //to generate the far edge values
                for (int y = (x + halfSide) % sideLength; y < size; y += sideLength)
                {
                    //x, y is center of diamond
                    //note we must use mod  and add DATA_SIZE for subtraction 
                    //so that we can wrap around the array to find the corners
                    float avg =
                      noise[(x - halfSide + (size + 1)) % (size + 1), y] + //left of center
                      noise[(x + halfSide) % (size + 1), y] + //right of center
                      noise[x, (y + halfSide) % (size + 1)] + //below center
                      noise[x, (y - halfSide + (size + 1)) % (size + 1)]; //above center
                    avg /= 4.0f;

                    //new value = average plus random offset
                    //We calculate random value in range of 2h
                    //and then subtract h so the end value is
                    //in the range (-h, +h)
                    avg = avg + ((float)r.NextDouble() * 2f * h) - h;
                    //update value for center of diamond
                    noise[x, y] = avg;

                    //wrap values on the edges, remove
                    //this and adjust loop condition above
                    //for non-wrapping values.
                    if (x == 0) noise[size, y] = avg;
                    if (y == 0) noise[x, size] = avg;
                }
            }
        }

        return noise;
    }
}
