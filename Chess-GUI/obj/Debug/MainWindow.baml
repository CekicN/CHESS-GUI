   M S B A M L   `   `   `  ����D  @Chess-GUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:Chess_GUI	Chess_GUI  5   	       Chess_GUI.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   X TXceed.Wpf.Toolkit, Version=4.6.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44wpfx)http://schemas.xceed.com/wpf/xaml/toolkit  5   	   #localclr-namespace:Chess_GUI  5   	     � Title$  	Chess-GUI��5	   	   $	��950��6   $	��830��6(    � 
ResizeMode$ NoResize=�5
   	    � WindowStartupLocation$ CenterScreen=�5   	    � Icon$, &/assets/chess_icon-icons.com_65163.ico��5   	   .��5      ]� $	��935��6   $	��830��6   $	��Top=�6)   .+�5   
   � $��20��6   $	��Top=�6    � RowDefinitions 5      �� 5      $��* �6     � ColumnDefinitions 5      �� 5      $
��1.5* �6#   �� 5      $��1* �6#   �� 5      $
��0.2* �6#   �� 5      $
��0.2* �6#   .+�5      o� .@�5      m� $��_BoardColor��6    m� Icon 5      �� 5      $��/assets/color.png��6    .@�5      #  Xceed.Wpf.Toolkit.ColorPicker  -   5        $��ColorPicker1��5   '   $��0q�6;   $	��100��6F   $��20��6R   6^     5      -   5        $��ColorPicker2��5   '   $��0q�6;   $	��100��6F   $��20��6R   6^   5       m� 5!      $��_Game��6   .@�5"      m� $��_LoadFromFile��6   .@�5#      	� -   5         	�Name$	 rb1��5#   &   .��6J   Load from file6Z   	� 5$      -   5        $	 rb2��5$   &    d� 	IsChecked$
 TrueY�6J   .��6[   Mouse6b   5%      m� 5&      -   5        $
��_New��5&      6-   m� 5'      -   5        $��_Rotate��5'      60   5(      5)      �� 5*      -   5        	 ��Name$	 Timer��5*   )   $��1��6   $��18(�68   $��Center=�6F   $��Center=�6a   $��00:00��6~   �� 5+      -   5        $��20��5+      $��2��6    $
��Left=�60   $��l��6K   6W   �� 5,      -	   5        $��20��5,      $��3��6    $
��Left=�60   $��>��6K   6W   5-      � 5.   
   .+�5/      �� -
   5        
 ��Name$
 canvas��5/      $	��830��6%   $
��Left=�62   $	��Top=�6M   6g   � 51      -   5         �Name$ 	BoardGrid��51      $	��800��6G   $	��800��6S   $
��Left=�6`   $	��Top=�6{   .+�52      A� -   5         A�Name$ HighlightGrid��54      $	��800��53      $	��800��6!    A� Rows$ 8��55       A� Columns$ 8��6   $
��Left=�56      $	��Top=�60    �� BitmapScalingMode$ HighQuality=�57      58      A� 59      -   5        $ 	PieceGrid��5;      $	��800��5:      $	��800��6!   $ 8��5<      $ 8��6   $
��Left=�5=      $	��Top=�60   $ HighQuality=�5>      5?      5@      �� 5A      -   5         ��Name$ MenuContainer��5A      �� 5C      -   5         ��Name$ scroll��5C      $��Bottom=�6+   $
��Auto=�6F   $	��100��6i   .��5D      �� -   5        $	 InfoTextBlock��5D      $
��Auto=�63   $
��Wrap=�6c   
����ح�$��16(�6�   $��Bottom=�6�   $	��100��6�   5E      5F      5G      5H      