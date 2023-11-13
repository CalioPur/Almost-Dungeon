using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogByteType 
{
    public static HashSet<int> fogFull = new HashSet<int>()
    {
        0b11111111
    };
    public static HashSet<int> fogTop= new HashSet<int>()
    {
        0b11110111,
        0b11110011,
        0b11100111,
        0b11100011,
        0b11101011
    };
    public static HashSet<int> fogRight= new HashSet<int>()
    {
        0b11111101,
        0b11111100,
        0b11111001,
        0b11111000,
        0b11111010
    };
    public static HashSet<int> fogBottom= new HashSet<int>()
    {
        0b01111111,
        0b01111110,
        0b00111111,
        0b00111110,
        0b10111110
    };
    public static HashSet<int> fogLeft = new HashSet<int>()
    {
        0b11011111,
        0b10011111,
        0b00111111,
        0b10001111,
        0b11001111,
        0b10101111
    };
    public static HashSet<int> fogTopLeft= new HashSet<int>()
    {
        0b11101111
    };
    public static HashSet<int> fogTopRight= new HashSet<int>()
    {
        0b11111011
    };
    public static HashSet<int> fogBottomRight= new HashSet<int>()
    {
        0b11111110
    };
    public static HashSet<int> fogBottomLeft= new HashSet<int>()
    {
        0b10111111
    };
    public static HashSet<int> fogCornerTopLeft= new HashSet<int>()
    {
        0b11000111,
        0b10000111,
        0b11000011,
        0b10000011,
        0b11010111,
        0b10101011
    };
    public static HashSet<int> fogCornerTopRight= new HashSet<int>()
    {
        0b11110001,
        0b11100001,
        0b11110000,
        0b11100000,
        0b11110101,
        0b11101010
    };
    public static HashSet<int> fogCornerBottomRight= new HashSet<int>()
    {
        0b01111100,
        0b00111100,
        0b01111000,
        0b00111000,
        0b01111101,
        0b10111010
    };
    public static HashSet<int> fogCornerBottomLeft= new HashSet<int>()
    {
        0b00011111,
        0b00011110,
        0b00001111,
        0b00001110,
        0b01011111,
        0b10101110
    };
    public static HashSet<int> fogSlash= new HashSet<int>()
    {
        0b11101110
    };
    public static HashSet<int> fogBackSlash= new HashSet<int>()
    {
        0b10111011
    };
    
}
