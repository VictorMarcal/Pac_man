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
            if (avisoBomba == null) { }
            avisoBomba = content.Load<SoundEffect>("som\\avisodaBomba");
            avisoBomba.Play(0.05f, 1, 0f);
        }

        public static void playExplosao(ContentManager content)
        {
            if (explosao == null)
                explosao = content.Load<SoundEffect>("som\\explosao");
            explosao.Play(0.3f, 1, 0f);
        }

        public static void playComer(ContentManager content)
        {
            if (pacmanComer == null)
                pacmanComer = content.Load<SoundEffect>("som\\pacmanComer");
            pacmanComer.Play(0.3f, 1, 0f);
        }
    }
}
