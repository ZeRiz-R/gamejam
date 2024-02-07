using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelesteLike
{
    public static class SoundBank
    {
        private static List<Sound> sounds;
        public static Sound button_press; // Menu
        public static Sound player_jump, player_hurt, player_death, player_dash; // Player
        public static Sound collect_coin; // Coin
        public static Sound bumper; // Bumper

        public static void LoadContent(ContentManager theContentManager)
        {
            sounds = new List<Sound>();
            button_press = new Sound("button_press", ref sounds);
            player_jump = new Sound("player_jump", ref sounds);
            player_hurt = new Sound("player_hurt", ref sounds);
            player_death = new Sound("player_death", ref sounds);
            collect_coin = new Sound("collect_coin", ref sounds);
            bumper = new Sound("bumper", ref sounds);
            player_dash = new Sound("Explosion2", ref sounds);

            foreach (Sound sound in sounds)
            {
                sound.LoadContent(theContentManager);
            }
        }
    }
}
