using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
  // Description: A static access to the human player.
  public static class Computer
  {
    public static Player player { get; private set; }

    public static void InitializeUser(Player prefab)
    {
      player = UnityEngine.Object.Instantiate(prefab);
      player.Initialize(playerType: PlayerType.Computer);
    }
  }
}
