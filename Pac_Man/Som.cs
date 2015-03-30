using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pac_Man
{
    /// <summary>
    /// Classe Som
    /// </summary>
    public static class Som
    {
        private static SoundEffect avisoBomba;
        private static SoundEffect explosao;
        private static SoundEffect pacmanComer;
        private static SoundEffect erroBomba;
        private static SoundEffect somTeleport;
        private static Song musica;


        /// <summary>
        /// Toca o som de aviso de bomba
        /// </summary>
        /// <param name="content">Instância de ContentManager</param>
        public static void playAvisoBomba(ContentManager content)
        {
            if (avisoBomba == null) { }
            avisoBomba = content.Load<SoundEffect>("som\\avisodaBomba");
            avisoBomba.Play(0.05f, 1, 0f);
        }

        /// <summary>
        /// Toca o som de explosão
        /// </summary>
        /// <param name="content">Instância de ContentManager</param>
        public static void playExplosao(ContentManager content)
        {
            if (explosao == null)
                explosao = content.Load<SoundEffect>("som\\explosao");
            explosao.Play(0.3f, 1, 0f);
        }

        /// <summary>
        /// Toca o som de comer
        /// </summary>
        /// <param name="content">Instância de ContentManager</param>
        public static void playComer(ContentManager content)
        {
            if (pacmanComer == null)
                pacmanComer = content.Load<SoundEffect>("som\\pacmanComer");
            pacmanComer.Play(0.3f, 1, 0f);
        }
        /// <summary>
        /// Toca o som de erro ao colocar bomba, quando nao tem pontuacao suficiente
        /// </summary>
        /// <param name="content">Instância de ContentManager</param>
        public static void playErro(ContentManager content)
        {
            if (erroBomba == null)
                erroBomba = content.Load<SoundEffect>("som\\error");
            erroBomba.Play(0.3f, 1, 0f);
        }
        public static void playTeleport(ContentManager content)
        {
            //if (somTeleport == null)
                somTeleport = content.Load<SoundEffect>("som\\somteleport");
            somTeleport.Play(0.1f, 1, 0f);
        }
        /// <summary>
        /// musica
        /// </summary>
        /// <param name="content">Instância de ContentManager</param>
        public static void playMusica(ContentManager content)
        {
            
                musica = content.Load<Song>("som\\musicamp3");
                MediaPlayer.Volume = 0.2f;
            MediaPlayer.Play(musica);
            MediaPlayer.IsRepeating = true;
           
            
        }
    }
}
