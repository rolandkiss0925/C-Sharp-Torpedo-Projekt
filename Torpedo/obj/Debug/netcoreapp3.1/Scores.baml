   M S B A M L   `   `   `  ????B  >Torpedo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:TorpedoTorpedo  5          Torpedo.Scores   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      'localclr-namespace:Torpedo     5        N? Title$  Scores??5	      .??5      ?  ? RowDefinitions 5   
   ?? 5      $
??0.5* ?6   6,   ?? 5      $??1* ?6   6*   ?? 5      $??1* ?6   6*   .+?5   
   ?? ???#446176$??Scores:??6%   $??16(?67   $??20,0,0,0q?6E   $
??Left=?6W   $??Center=?6r   $??0??6?   &   System.Windows.Controls.DataGrid  5   
   $??1??6   ???#2c2137???#446176   VerticalGridLinesBrush ?#446176   HorizontalGridLinesBrush ?#446176$??	20,0,20,0q?6?    ? 	Resources 5      =  7System.Windows.Controls.Primitives.DataGridColumnHeader ?? BasedOn ?? 
TargetType ?? Property ?? Value??%?   '        7 ?? 5      8   "  6Q   .?5      ??  ? ?? ?#44617663   ?? 5       ? ?? ?#2c213763   5      5      	   Columns	 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn
   Header$

 Name??6%      
IsReadOnly$
 True??63      Width5  /System.Windows.Controls.DataGridLengthConverter$ 1* 6E   6R     5      $
 Rounds??6%   $
 True??65   $ 1* 6G   6T     5      $
 Player's hits??6%   $
 True??6<   $ 1* 6N   6[     5      $
 Winner??6%   $
 True??65   $ 1* 6G   6T   5"      ?? 5$   
   -   5        $??Cancel??5$      $??2??63   $
??Left=?6@   $??Bottom=?6\   $	??120??6w   $??32??6?   $??0q?6?   $??	50,0,0,35q?6?   ???#446176???#3fac95???#3fac956?   5%      5&      