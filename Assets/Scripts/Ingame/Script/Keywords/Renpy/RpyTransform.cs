using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy
{
    public class RpyTransform : IStatement
    {
        public int Line { get; set; } = 0;
        public string Name { get; set; }
        public bool IsGlobal { get; set; } = false;

        public float xpos = -1f;
        public float ypos = -1f;
        public float xalign = -1f;
        public float yalign = -1f;
        public float xcenter = -1f;
        public float ycenter = -1f;
        public float xanchor = 0f;
        public float yanchor = 0f;

        public float zoom = 1f;

        /// <summary>
        /// -1: infinite loop, 0: no repeat, 0 ~: repeat count
        /// </summary>
        public int repeat = 0;
        public IExpression repeatAsExpression = null;

        /// <summary>
        /// linear, ease, easein, easeout etc
        /// </summary>
        public string easeName = "";
        public float easeDuration = 0f;
        public IExpression easeDurationAsExpression = null;

        //Custom syntax
        public string colour = "";

        public RpyTransform()
        {

        }

        public void Interpret()
        {
            if (string.IsNullOrEmpty(Name))
            {
                ExceptionManager.Throw("The transform name can't be empty.", "Script/RpyTransform", Line);
                return;
            }

            if (repeatAsExpression != null)
            {
                float? value = repeatAsExpression.Interpret() as float?;
                if (value.HasValue) repeat = (int)value.Value;
            }

            var vars = IsGlobal ? IngameManagerV2.Global : IngameManagerV2.Local;

            if (vars.Transforms.ContainsKey(Name)) vars.Transforms[Name] = this; //overwrite
            else vars.Transforms.Add(Name, this);
        }
    }
}