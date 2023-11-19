using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogByteType 
{
    public static HashSet<int> fogEncapsulated= new HashSet<int>()
    {
        0b11111111,
        0b10111111,
        0b11101111,
        0b11111011,
        0b11111110,
        0b10101010,
        0b10101011,
        0b10101110,
        0b10111010,
        0b11101010,
        0b11111010,
        0b11101110,
        0b11101011,
        0b10111110,
        0b10111011,
        0b10101111,
    };
    public static HashSet<int> fogTop= new HashSet<int>()
    {
        0b11000001
    };
    public static HashSet<int> fogRight= new HashSet<int>()
    {
        0b01110000
    };
    public static HashSet<int> fogBottom= new HashSet<int>()
    {
        0b00011100
    };
    public static HashSet<int> fogLeft = new HashSet<int>()
    {
        0b00000111
    };
    public static HashSet<int> fogTopLeft= new HashSet<int>()
    {
        0b11000111,
        0b10000111,
        0b11000011,
        0b10000011
    };
    public static HashSet<int> fogTopRight= new HashSet<int>()
    {
        0b11110001,
        0b11100001,
        0b11110000,
        0b11100000
    };
    public static HashSet<int> fogBottomRight= new HashSet<int>()
    {
        0b01111100,
        0b01111000,
        0b00111100,
        0b00111000
    };
    public static HashSet<int> fogBottomLeft= new HashSet<int>()
    {
        0b00011111,
        0b00011110,
        0b00001111,
        0b00001110
    };
    public static HashSet<int> fogCornerTopLeft= new HashSet<int>()
    {
        0b00000001,
    };
    public static HashSet<int> fogCornerTopRight= new HashSet<int>()
    {
        0b01000000,
    };
    public static HashSet<int> fogCornerBottomRight= new HashSet<int>()
    {
        0b00010000,
    };
    public static HashSet<int> fogCornerBottomLeft= new HashSet<int>()
    {
        0b00000100,
    };
    public static HashSet<int> fogSlash= new HashSet<int>()
    {
        0b01000100
    };
    public static HashSet<int> fogBackSlash= new HashSet<int>()
    {
        0b00010001
    };
    public static HashSet<int> fogTopEncapsulated = new HashSet<int>()
    {
        0b11110111,
        0b11100111,
        0b11110011,
        0b11100011
    };
    public static HashSet<int> fogRightEncapsulated = new HashSet<int>()
    {
        0b11111101,
        0b11111001,
        0b11111100,
        0b11111000
    };
    public static HashSet<int> fogBottomEncapsulated = new HashSet<int>()
    {
        0b01111111,
        0b01111110,
        0b00111111,
        0b00111110
    };
    public static HashSet<int> fogLeftEncapsulated = new HashSet<int>()
    {
        0b11011111,
        0b10011111,
        0b11001111,
        0b10001111
    };
    public static HashSet<int> fogTopBottom = new HashSet<int>()
    {
        0b11011101,
        0b10011101,
        0b11001101,
        0b10001101,
        0b11011001,
        0b10011001,
        0b11001001,
        0b10001001,

        0b11011100,
        0b10011100,
        0b11001100,
        0b10001100,
        0b11011000,
        0b10011000,
        0b11001000,
        0b10001000
    };
    public static HashSet<int> fogRightLeft = new HashSet<int>()
    {
        0b01110111,
        0b01100111,
        0b01110011,
        0b01100011,
        0b01110110,
        0b01100110,
        0b01110010,
        0b01100010,

        0b00110111,
        0b00100111,
        0b00110011,
        0b00100011,
        0b00110110,
        0b00100110,
        0b00110010,
        0b00100010
    };

    public static HashSet<int> fogDoubleCornerTop = new HashSet<int>()
    {
        0b01000001
    };
    public static HashSet<int> fogDoubleCornerRight = new HashSet<int>()
    {
        0b01010000
    };
    public static HashSet<int> fogDoubleCornerBottom = new HashSet<int>()
    {
        0b00010100
    };
    public static HashSet<int> fogDoubleCornerLeft = new HashSet<int>()
    {
        0b00000101
    };


    public static HashSet<int> fogTripleCornerTopLeft = new HashSet<int>()
    {
        0b01000101
    };
    public static HashSet<int> fogTripleCornerTopRight = new HashSet<int>()
    {
        0b01010001
    };
    public static HashSet<int> fogTripleCornerBottomRight = new HashSet<int>()
    {
        0b01010100
    };
    public static HashSet<int> fogTripleCornerBottomLeft = new HashSet<int>()
    {
        0b00010101
    };

    public static HashSet<int> fogQuadCorner= new HashSet<int>()
    {
        0b01010101
    };

    public static HashSet<int> fogTurnTopLeft = new HashSet<int>()
    {
        0b11010111,
        0b10010111,
        0b11010011,
        0b10010011
    };
    public static HashSet<int> fogTurnTopRight = new HashSet<int>()
    {
        0b11110101,
        0b11100101,
        0b11110100,
        0b11100100
    };
    public static HashSet<int> fogTurnBottomRight = new HashSet<int>()
    {
        0b01111101,
        0b01111001,
        0b00111101,
        0b00111001
    };
    public static HashSet<int> fogTurnBottomLeft = new HashSet<int>()
    {
        0b01011111,
        0b01011110,
        0b01001111,
        0b01001110
    };


    public static HashSet<int> cornerLeftBottom = new HashSet<int>()
    {
        0b00011101,
        0b00001101,
        0b00011001,
        0b00001001
    };
    public static HashSet<int> cornerLeftLeft = new HashSet<int>()
    {
        0b01000111,
        0b01000011,
        0b01000110,
        0b01000010
    };
    public static HashSet<int> cornerLeftTop = new HashSet<int>()
    {
        0b11010001,
        0b11010000,
        0b10010001,
        0b10010000
    };
    public static HashSet<int> cornerLeftRight = new HashSet<int>()
    {
        0b01110100,
        0b00110100,
        0b01100100,
        0b00100100
    };


    public static HashSet<int> cornerRightBottom = new HashSet<int>()
    {
        0b01011100,
        0b01001100,
        0b01011000,
        0b01001000
    };
    public static HashSet<int> cornerRightLeft = new HashSet<int>()
    {
        0b00010111,
        0b00010011,
        0b00010110,
        0b00010010
    };
    public static HashSet<int> cornerRightTop = new HashSet<int>()
    {
        0b11000101,
        0b11000100,
        0b10000101,
        0b10000100
    };
    public static HashSet<int> cornerRightRight = new HashSet<int>()
    {
        0b01110001,
        0b00110001,
        0b01100001,
        0b00100001
    };


    public static HashSet<int> doubleCornerBottom = new HashSet<int>()
    {
        0b01011101,
        0b01001101,
        0b01011001,
        0b01001001
    };
    public static HashSet<int> doubleCornerLeft = new HashSet<int>()
    {
        0b01010111,
        0b01010011,
        0b01010110,
        0b01010010
    };
    public static HashSet<int> doubleCornerTop = new HashSet<int>()
    {
        0b11010101,
        0b11010100,
        0b10010101,
        0b10010100
    };
    public static HashSet<int> doubleCornerRight = new HashSet<int>()
    {
        0b01110101,
        0b00110101,
        0b01100101,
        0b00100101
    };
}
