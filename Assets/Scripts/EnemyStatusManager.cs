using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusManager : Singleton<EnemyStatusManager>
{
    public string Ename;        // “G‚Ì–¼‘O
    public int EmaxHP;          // “GÅ‘åHP
    public int EcurrentHP;      // Œ»İ‚Ì“GHP
    public int EmaxMP;          // “GÅ‘åMP
    public int EcurrentMP;      // Œ»İ‚Ì“GMP
    public int EattackPower;    // “GUŒ‚—Í
    public int EdefensePower;   // “G–hŒä—Í
    public int Espeed;          // “G‘f‘‚³
    public int Eexp;            // “G‚ÌŒoŒ±’l
    public int Egold;           // “G‚Ì‚¨‹à

    public void Slime()
    {
        Ename = "ƒXƒ‰ƒCƒ€";
        EmaxHP = 10;
        EcurrentHP = 10;
        EmaxMP = 0;
        EcurrentMP = 0;
        EattackPower = 3;
        EdefensePower = 0;
        Espeed = 1;
        Eexp = 2;
        Egold = 10;
    }

    public void Alraune()
    {
        Ename = "ƒAƒ‹ƒ‰ƒEƒl";
        EmaxHP = 5;
        EcurrentHP = 5;
        EmaxMP = 5;
        EcurrentMP = 5;
        EattackPower = 3;
        EdefensePower = 2;
        Espeed = 10;
        Eexp = 5;
        Egold = 20;
    }

    public void Kinoko()
    {
        Ename = "ƒLƒmƒR";
        EmaxHP = 15;
        EcurrentHP = 15;
        EmaxMP = 0;
        EcurrentMP = 0;
        EattackPower = 6;
        EdefensePower = 0;
        Espeed = 1;
        Eexp = 5;
        Egold = 20;
    }

    public void Ahriman()
    {
        Ename = "ƒA[ƒŠƒ}ƒ“";
        EmaxHP = 70;
        EcurrentHP = 70;
        EmaxMP = 10;
        EcurrentMP = 10;
        EattackPower = 45;
        EdefensePower = 10;
        Espeed = 20;
        Eexp = 50;
        Egold = 50;
    }

    public void Bomb()
    {
        Ename = "ƒ{ƒ€";
        EmaxHP = 100;
        EcurrentHP = 100;
        EmaxMP = 0;
        EcurrentMP = 0;
        EattackPower = 30;
        EdefensePower = 5;
        Espeed = 1;
        Eexp = 75;
        Egold = 80;
    }

    public void Ghost()
    {
        Ename = "ƒS[ƒXƒg";
        EmaxHP = 50;
        EcurrentHP = 50;
        EmaxMP = 20;
        EcurrentMP = 20;
        EattackPower = 30;
        EdefensePower = 15;
        Espeed = 10;
        Eexp = 75;
        Egold = 80;
    }

    public void Ricchi()
    {
        Ename = "ƒŠƒbƒ`";
        EmaxHP = 200;
        EcurrentHP = 200;
        EmaxMP = 500;
        EcurrentMP = 500;
        EattackPower = 30;
        EdefensePower = 30;
        Espeed = 100;
        Eexp = 100;
        Egold = 100;
    }
}