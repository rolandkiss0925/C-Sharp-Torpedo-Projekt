   M S B A M L   `   `   `  ????B  >Torpedo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:TorpedoTorpedo  5   	       Torpedo.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:Torpedo     5   	   -   5          ? Title$  Torpedo??5   	    ? 
ResizeMode$ NoResize=?5	   	    ? WindowStartupLocation$ CenterScreen=?5
   	    ? SizeToContent$ WidthAndHeight=?5   	   .??5      ?? ???#2c2137.+?5   
   ?? ???#446176.+?5      ??  ?? Orientation$ 
Horizontal=?6   .+?5      ?? $??Turn: ??6   $
??Wrap=?6*   $??5q?6>   ?? 5      -   5        $??TurnTextBlock??5      $
??Wrap=?62   $??5q?6F   ?? 5      $??Hits: ??6   $
??Wrap=?6*   $??5q?6>   ?? 5      -   5        $??HitsTextBlock??5      $
??Wrap=?62   $??5q?6F   ?? 5      $??Current Player: ??6   $
??Wrap=?64   $??	10 5 10 5q?6H   ?? 5      -   5        $ ??CurrentPlayerNameTextBlock??5      $
??Wrap=?6?   $??5q?6S   5      5      ?? 5   
   $ 
Horizontal=?6   .+?5      ?? -   5        $??EnemyCanvas??5      $	??300??6^   $	??300??6k   $??
16 16 8 16q?6w   ??5      L? 5       j? TileMode$
 Tile=?6#    j? Viewport$ 	0,0,30,30?63    j? ViewportUnits$ Absolute=?6H    L? Drawing 5      ? 5      	 ? Geometry	 5   "   ?? 5   &   
 ?? Rect$
 	0,0,40,40?68   
 ? Pen 5!   "   >? 5"   &    >? Brush ?#3fac95 >? 	Thickness$ 1Q?6:   5$      5&      5(      ?? 5)      -   5        $??	OwnCanvas??5)      $	??300??6&   $	??300??63   $??	8 16 8 16q?6?   ??5*      L? 5+      $
 Tile=?6#   $ 	0,0,30,30?63   $ Absolute=?6H    5,      ? 5-      	 5.   "   ?? 5/   &   $
 	0,0,40,40?68    51   "   >? 52   &    ?#3fac95$ 1Q?6:   54      56      58      ?? 59      -   5        $??RemainingCanvas??59      $	??300??6,   $	??150??69   $??
8 16 16 16q?6E   ??5:      L? 5;      $
 Tile=?6#   $ 	0,0,30,30?63   $ Absolute=?6H    5<      ? 5=      	 5>   "   ?? 5?   &   $
 	0,0,50,50?68    5A   "   >? 5B   &    ?#3fac95$ 1Q?6:   5D      5F      5H      5I      ?? 5J   
   $ 
Horizontal=?6   $??Center=?6.   .+?5K      ?? -   5        $??AiShotButton??5K      $??See AI's Shot??6)   $??80??6W   $??32??6b   $??0q?6n   $??5,0,5,10q?6z   ???#446176???#3fac95???#3fac956?   5L      5M      5P      