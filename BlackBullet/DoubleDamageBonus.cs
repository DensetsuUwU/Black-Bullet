using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace BlackBullet
{
    public class DoubleDamageBonus : Bonus
    {

        public DoubleDamageBonus(Texture2D _sprite, Vector2 _position) : base(_sprite, _position, 5, 0, 2)
        {
            
        }
    }
}
