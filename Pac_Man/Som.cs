using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pac_Man
{
    public static class Som
    {
        private static SoundEffect avisoBomba;
        private static SoundEffect explosao;
        private static SoundEffect pacmanComer;

    

        public static void playAvisoBomba(ContentManager content)
        {
            avisoBomba = content.Load<SoundEffect>("som\\avisodaBomba");
            
            avisoBomba.Play(.1f,1,1);
        }

        public static void playExplosao(ContentManager content)
        {
            explosao = content.Load<SoundEffect>("som\\explosao");
            explosao.Play(1,1,1);
        }

        public static void playComer(ContentManager content)
        {
            pacmanComer = content.Load<SoundEffect>("som\\pacmanComer");
            pacmanComer.Play();
        }
    }
}
