using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class InfoLevel
{
    public int level;
    public int star;

    public InfoLevel() {

    }

    public InfoLevel(int _level, int _star) {
        level = _level;
        star = _star;
    }
}
