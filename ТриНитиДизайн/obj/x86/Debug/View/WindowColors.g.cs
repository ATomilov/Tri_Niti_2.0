﻿#pragma checksum "..\..\..\..\View\WindowColors.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F40270FFB582A743311A9A02D582F264"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using ТриНитиДизайн;


namespace ТриНитиДизайн {
    
    
    /// <summary>
    /// WindowColors
    /// </summary>
    public partial class WindowColors : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\..\..\View\WindowColors.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button button_accept;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\..\..\View\WindowColors.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button button_cancel;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\..\..\View\WindowColors.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button button_delete;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\..\View\WindowColors.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Rectangle Rect_main;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\..\View\WindowColors.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas canvasColors;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ТриНитиДизайн;component/view/windowcolors.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\View\WindowColors.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.button_accept = ((System.Windows.Controls.Button)(target));
            
            #line 10 "..\..\..\..\View\WindowColors.xaml"
            this.button_accept.Click += new System.Windows.RoutedEventHandler(this.button_accept_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.button_cancel = ((System.Windows.Controls.Button)(target));
            
            #line 11 "..\..\..\..\View\WindowColors.xaml"
            this.button_cancel.Click += new System.Windows.RoutedEventHandler(this.button_cancel_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.button_delete = ((System.Windows.Controls.Button)(target));
            
            #line 12 "..\..\..\..\View\WindowColors.xaml"
            this.button_delete.Click += new System.Windows.RoutedEventHandler(this.button_delete_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Rect_main = ((System.Windows.Shapes.Rectangle)(target));
            return;
            case 5:
            this.canvasColors = ((System.Windows.Controls.Canvas)(target));
            
            #line 14 "..\..\..\..\View\WindowColors.xaml"
            this.canvasColors.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Canvas_MouseLeftButtonDown);
            
            #line default
            #line hidden
            
            #line 14 "..\..\..\..\View\WindowColors.xaml"
            this.canvasColors.MouseRightButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Canvas_MouseRightButtonDown);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

