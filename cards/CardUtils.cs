using System.Collections.Generic;
using System.IO;
using APIPlugin;
using BepInEx;
using DiskCardGame;
using UnityEngine;
using static System.IO.File;

namespace LifeCost.cards
{
    public static class CardUtils
    {
		public static Texture2D GetTextureFromPath(string path)
		{
			byte[] imgBytes = File.ReadAllBytes(Path.Combine(Plugin.Directory, path));
			Texture2D tex = new Texture2D(2, 2);
			tex.LoadImage(imgBytes);
			return tex;
		}

	}
}
