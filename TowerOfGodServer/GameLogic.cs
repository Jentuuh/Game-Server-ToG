﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TowerOfGodServer
{
     /*
     * @author Jente Vandersanden
     * 18/08/2020
     * Class that represents the game's server side logic.
     */
    class GameLogic
    {
        public static void Update()
        {
            ThreadManager.UpdateMain();
        }
    }
}
