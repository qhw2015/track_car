// Two Cameras Vision
//
// Copyright © Andrew Kirillov, 2009
// andrew.kirillov@aforgenet.com
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Gesture
{
    public partial class DetectedObjectsForm : Form
    {
        public DetectedObjectsForm( )
        {
            InitializeComponent( );
        }

        // On form closing - hide it instead
        private void DetectedObjectsForm_FormClosing( object sender, FormClosingEventArgs e )
        {
            this.Hide( );
            e.Cancel = true;
        }

    }
}
