using System;

using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace BarcodeReader_ce_CF2
{
        
    class Sound
    {
        private byte[] m_soundBytes;
        private string m_fileName;
        public int[] volumes = { 0, 858993459, 1717986918, -1717986919, -858993460, -1 };
    
        private enum Flags
        {
            SND_SYNC = 0x0000,  /* play synchronously (default) */
            SND_ASYNC = 0x0001,  /* play asynchronously */
            SND_NODEFAULT = 0x0002,  /* silence (!default) if sound not found */
            SND_MEMORY = 0x0004,  /* pszSound points to a memory file */
            SND_LOOP = 0x0008,  /* loop the sound until next sndPlaySound */
            SND_NOSTOP = 0x0010,  /* don't stop any currently playing sound */
            SND_NOWAIT = 0x00002000, /* don't wait if the driver is busy */
            SND_ALIAS = 0x00010000, /* name is a registry alias */
            SND_ALIAS_ID = 0x00110000, /* alias is a predefined ID */
            SND_FILENAME = 0x00020000, /* name is file name */
            SND_RESOURCE = 0x00040004  /* name is resource name or atom */
        }

        [DllImport("CoreDll.DLL", EntryPoint = "PlaySound", SetLastError = true)]
        private extern static int WCE_PlaySound(string szSound, IntPtr hMod, int flags);

        [DllImport("CoreDll.DLL", EntryPoint = "PlaySound", SetLastError = true)]
        private extern static int WCE_PlaySoundBytes(byte[] szSound, IntPtr hMod, int flags);

        [DllImport("coredll.dll", EntryPoint = "waveOutSetVolume", SetLastError = true)]
        internal static extern int SetVolume(IntPtr device, int volume);

        [DllImport("coredll.dll", EntryPoint = "waveOutGetVolume", SetLastError = true)]
        internal static extern int GetVolume(IntPtr device, ref int volume);


        /// <summary> 
        /// Construct the Sound object to play sound data from the specified file. 
        /// </summary> 
        public Sound(string fileName)
        {
            m_fileName = fileName;
        }

        

        
        /// <summary> 
        /// Play the sound 
        /// </summary> 
        public void Play()
        {
            // if a file name has been registered, call WCE_PlaySound, 
            //  otherwise call WCE_PlaySoundBytes 
            if (m_fileName != null)
                WCE_PlaySound(m_fileName, IntPtr.Zero, (int)(Flags.SND_ASYNC | Flags.SND_FILENAME));
            else
                WCE_PlaySoundBytes(m_soundBytes, IntPtr.Zero, (int)(Flags.SND_ASYNC | Flags.SND_MEMORY));
        }

        public int Volume
        {
            get
            {
                int v = (int)0;

                GetVolume(IntPtr.Zero, ref v);

                if (v == volumes[0])
                    return 0;
                else if (v == volumes[1])
                    return 1;
                else if (v == volumes[2])
                    return 2;
                else if (v == volumes[3])
                    return 3;
                else if (v == volumes[4])
                    return 4;
                else
                    return 5;

            }
            set
            {
                int v = 0;
                if (value == 0)
                    v = volumes[0];
                else if (value == 1)
                    v = volumes[1];
                else if (value == 2)
                    v = volumes[2];
                else if (value == 3)
                    v = volumes[3];
                else if (value == 4)
                    v = volumes[4];
                else
                    v = volumes[5];

                SetVolume(IntPtr.Zero, v);
            }
        }
    }
}
