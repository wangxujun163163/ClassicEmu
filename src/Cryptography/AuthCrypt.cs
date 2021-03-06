﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Classic.Cryptography
{
    // Based on
    // https://github.com/cmangos/mangos-classic/blob/master/src/shared/Auth/AuthCrypt.cpp
    public class AuthCrypt
    {
        private byte[] key;
        private int send_i = 0;
        private int send_j = 0;
        private int recv_i = 0;
        private int recv_j = 0;

        public bool IsInitialized { get; private set; }

        public byte[] Decrypt(byte[] data, int size)
        {
            for (int i = 0; i < size; i++)
            {
                this.recv_i %= (byte)this.key.Length;
                var x = (byte)((data[i] - this.recv_j) ^ this.key[this.recv_i]);
                this.recv_i++;
                this.recv_j = data[i];
                data[i] = x;
            }

            return data;
        }

        public byte[] Encrypt(byte[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                this.send_i %= (byte)this.key.Length;
                var x = (byte)((data[i] ^ this.key[this.send_i]) + this.send_j);
                this.send_i++;
                data[i] = x;
                this.send_j = x;
            }

            return data;
        }

        public void SetKey(byte[] key)
        {
            this.key = key;
            this.IsInitialized = true;
        }
    }
}
