   M S B A M L   `   `   `  ����K  GVoaDownloaderWpf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:VoaDownloaderWpfVoaDownloaderWpf  5   	   &     VoaDownloaderWpf.VocabBookWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:VoaDownloaderWpf     5   	   _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5   	     � Title$  我的生词本��5	   	   $	��600��6   $	��800��6$    � WindowStartupLocation$ CenterScreen=�5
   	      BackgroundBrush#��[  5   	    � 	Resources 5      ��%    5      .��5      � $��15q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6     ContentBackgroundBrush#��[ 6    �� CornerRadius$ 4��6S    �� Padding$ 10q�6d   $��0,0,0,10q�6q   ._�5      �� $��显示已学会的单词��6   $��Center=�5      #��[B��5       d� 	IsChecked 5      �� *ShowLearned+5      5      &   System.Windows.Controls.DataGrid  5    
   $��1��6     ModernDataGridStyle#��[ 5!       # ModernDataGridColumnHeaderStyle   ColumnHeaderStyle# [ 5"        ModernDataGridRowStyle   RowStyle# [ 5#        ModernDataGridCellStyle	   	CellStyle#	 [ 5$      
   AutoGenerateColumns$
 False��5%         CanUserAddRows$ False��5&         HeadersVisibility$ Column=�5'         SelectionMode$ Extended=�5(      ��5        �� *VocabEntries+   Columns 5*      4  .System.Windows.Controls.DataGridTemplateColumn  5+      ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6)      Header 5,      �� 5-       6#   �� *DataContext.IsAllSelected+ �� RelativeSource ��  �� AncestorType DataGrid �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�   CellTemplate 52      �� 53      .R�54      �� $��Center=�55      $��Center=�56       54   '   �� *
IsSelected+$ PropertyChanged=���57      �� *%#DataContext.SelectionChangedCommand+ ��  DataGrid58      5:      0  *System.Windows.Controls.DataGridTextColumn  5<      $ 状态��6%   $
 Auto 6�      
FontWeight$
 Bold"�6�      
Foreground
 � � �1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   �� *	IsLearned+  LearnedStatusConverter �� 	Converter# [ 6�     5=      $ 单词��6%   $ * 6J    61   �� *Word+6J     5>      $ 释义��6%   $ 3* 6P    61   �� *
Definition+6P     5?      $ 学习次数��6%   $
 Auto 6S    63   �� *LookupCount+6S     5@      $ 添加日期��6%   $
 Auto 6l    63   �� *	DateAdded+ �� StringFormat$ 
yyyy-MM-dd��6l   5B      �� 5D   
   $��2��6   #��[ 6   $ 10q�6S   $��0,10,0,0q�6`     BorderBrush#��[ 5E   	   $��0,1,0,0q�64   ._�5F      �  � ColumnDefinitions 5G      �� 5H      $��* �6'   �� 5I      $
��Auto �6'   .+�5L      �� $��0��6    �� Orientation$ 
Horizontal=�6-   .+�5M      �� $��标记为“已学会”��6     IconFunctionButtonStyle#��[ 6a   
 � Tag$	 ��6�   ��60   �� *MarkSelectedAsLearnedCommand+y�5N      �� *ShowLearned+ '	 #InverseBooleanToVisibilityConverter# [	 �� 5P      $��标记为“未学会”��6   #��[ 6c   $	 ��6�   ��60   �� * MarkSelectedAsForgottenCommand+y�5Q      �� *ShowLearned+  
 BooleanToVisibilityConverter# [
 �� 5S      $��彻底删除��6    ! DangerIconFunctionButtonStyle#��[ 6V   $	 ��6�   $��20,0,0,0q�6�   ��6,   �� *DeleteSelectedCommand+6�   5T      �� 5V      $��1��6   $ 
Horizontal=�6-   .+�5W      ��   ReviewButtonStyle#��[ 5X       � ToolTip$* $从所有生词中随机开始复习��5Y      ��5W      �� *FreeReviewCommand+.��5Z      �� $ 
Horizontal=�6%   .+�5[      �� $	����6(     IconFont#��[ 68   $��Center=�6_   $��0,0,6,0q�6z   �� 5\      $��自由复习��6(   �� 5]      
��� E��$
��Bold"�6~   ��6(   �� *AllUnlearnedWordsCount+$  ({0})��y�5^       �� *AllUnlearnedWordsCount+  IntToVisibilityConverter# [ 5_      5`      �� 5b      #��[ 5c      $��10,0,0,0q�5d      $E ?根据艾宾浩斯遗忘曲线开始复习今天到期的单词��5e      ��5b      �� *EbbinghausReviewCommand+.��5f      �� $ 
Horizontal=�6%   .+�5g      �� $	����6(   #��[ 68   $��Center=�6_   $��0,0,6,0q�6z   �� 5h      $��今日计划��6(   �� 5i      
��� E��$
��Bold"�6z   ��6(   �� *WordsToReviewCount+$  ({0})��y�5j       �� *WordsToReviewCount+# [ 5k      5l      5m      5n      5o      5p      5q      