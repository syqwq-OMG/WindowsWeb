   M S B A M L   `   `   `  ���� K  GVoaDownloaderWpf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:VoaDownloaderWpfVoaDownloaderWpf  I)clr-namespace:VoaDownloaderWpf.ConvertersVoaDownloaderWpf.Converters  W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0899    3VoaDownloaderWpf.Converters.InverseBooleanConverter   InverseBooleanConverter>   8VoaDownloaderWpf.Converters.BooleanToVisibilityConverter   BooleanToVisibilityConverterE   ?VoaDownloaderWpf.Converters.InverseBooleanToVisibilityConverter ' #InverseBooleanToVisibilityConverter:   4VoaDownloaderWpf.Converters.ChatBubbleStyleConverter  ChatBubbleStyleConverter9   3VoaDownloaderWpf.Converters.ChatForegroundConverter  ChatForegroundConverter8   2VoaDownloaderWpf.Converters.LearnedStatusConverter  LearnedStatusConverter  PrimaryBrush  �� Color  PrimaryBrush.MouseOver  PrimaryBrush.Pressed 	 BackgroundBrush 
 ContentBackgroundBrush  BorderBrush  TextPrimaryBrush  TextRegularBrush  TextPlaceholderBrush  TextSecondaryBrush  IconFont  PrimaryButtonStyle �� 
TargetType  IconButtonStyle �� BasedOn  ActionButtonStyle  ModernDataGridStyle&   System.Windows.Controls.DataGrid �� Property �� Value # ModernDataGridColumnHeaderStyle=  7System.Windows.Controls.Primitives.DataGridColumnHeader  ModernDataGridRowStyle)  #System.Windows.Controls.DataGridRow  ModernDataGridCellStyle*	  $System.Windows.Controls.DataGridCell  ChatMessageBorderStyle  UserMessageStyle  AiMessageStyle  SendButtonStyle �� 
TargetType ��Name �� CornerRadius �� Triggers	 P� Property
 P� Value �� 
TargetName  IconFunctionButtonStyle �� Orientation
 � Tag  SmallIconButton  TabRadioButtonStyle  ModernCheckBoxStyle F�Name   DangerButtonStyle �� Triggers !! DangerIconFunctionButtonStyle " HintButtonStyle<
   6VoaDownloaderWpf.Converters.IsGreaterThanZeroConverter # IsGreaterThanZeroConverter:   4VoaDownloaderWpf.Converters.IntToVisibilityConverter $ IntToVisibilityConverter % ReviewButtonStyle & InteractiveCardStyle3  -System.Windows.Media.Effects.DropShadowEffect   ShadowDepth   
BlurRadius   Color   Opacity N� EnterActions �� TargetProperty	 W� To g� Duration N� ExitActions�� F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml     (localclr-namespace:VoaDownloaderWpf  8
converters)clr-namespace:VoaDownloaderWpf.Converters  %�  &	        &	      &	 
     &	      &	      &	      &	      &	 1     &	 D     &		 W     &	
 j     &	 }     &	 �     &	 �     &	 �     &	 �     &	 �     &	 �     &	     7  &	     '��     '�� $    &	 .    7 
 7  &	 �    &	 �    &	 �    &	 �    &	 v    7  7  &	 �    7  &	 �    7  7  7  7  &	 �    7  7  7  7  7  &	 �    7  7  7  7  &	 �
    7  7  7  &	 �    7 
 7  7  7  &	  �    7  &	! _    7  &	" K    7  7  7  7  7  &	# 8    &	$ =    &	% B    7  7  7  7  &	& �                 �� $  #007ACC���� $  #005A9E���� $  #003D6A���� $  #F0F2F5���� $  #FFFFFF���� $  #DCDFE6���� $  #303133���� $  #606266���� $  #C0C4CC���� $  #909399��*�Segoe MDL2 Assets)��� " ���� " ��8   �� " ���� " ���� " ���� "  .���  � ��8   ��  � ��$ 0q���   �@ HorizontalGridLinesBrush8  ��  �@ VerticalGridLinesBrush
 ���� ��  �@ 	RowHeight$ 35Q���   �@ AlternatingRowBackground �#F9FAFC��  �@ CanUserAddRows$ False����  �@ CanUserDeleteRows$ False����  �@ 
IsReadOnly$
 True����  �@ AutoGenerateColumns$ False����  �@ SelectionMode$ Extended=��� "  �� "  �� " 	 �� " ��.���  �@��CornerRadius$ 12����  �@��Padding$ 10q���  � ��$ 5q���  � ��$	 250���� " ��8   .���  � ��8  ��  � ��$ Right=��� " ��8   .���  � �� �#E5E5E5��  � ��$
 Left=��� " ��.���  � ��$ 35����  � ��$ 35����  � ��8   ��  � ��$ 16(���  � ��8  ��  � ��
 �������  � ��$ 0q���  �@�Cursor$
 Hand����  � �� �� " ��.R��� $ border��#��z��$ 20��._��� $	����#��z��#��z��#��z��$��Center=�$��Center=� P� 	 �@J�IsMouseOver$

 True��. ��� $ border��8   � ��P� 	 � ��$

 True��. ��� $ border��8   � ��P� 	 � {�$
 False��. ���  �@J�Opacity$	 0.5Q��� " ��.���  � ��$
 10,5q���  � ��$ 0,0,8,0q���  � ��$ 14(���  � ��
 ���� ��  � ��8   ��  � ��$ 1q���  � �� �#DDDDDD��  �@�Cursor$
 Hand����  � �� �� " ��.R��� $ border��#��z��#��z��#��z��$ 4��._��� $ 
Horizontal=�$��Center=�#��z��.+��� #��z 8�� $��Center=�$��0,0,6,0q�$��16(��� #��z��$��Center=� P� 	 �@J�IsMouseOver$

 True��. ��� $ border��8   � ���� 8   � ��P� 	 � ��$

 True��. ��� $ border�� � �� �#EAF4FC�� 8   � ��P� 	 � {�$
 False��. ���  �@J�Opacity$	 0.5Q��� " ��.���  � ��$ 30����  � ��$ 15����  � ��8   ��  � ��$ 8(���  � ��
 ���� ��  � ��8  ��  � ��$ 1q���  � �� �#DDDDDD��  �@�Cursor$
 Hand����  � �� �� " ��.R��� $ border��#��z��#��z��#��z��$ 3��._��� #��z��#��z��#��z��#��z��$��Center=�$��Center=� P� 	 �@J�IsMouseOver$

 True��. ��� $ border��8   � ���� 8   � ��P� 	 � ��$

 True��. ��� $ border�� � �� �#EAF4FCP� 	 � {�$
 False��. ���  �@J�Opacity$	 0.5Q��� " 	�.���  � ��$	 8,4q���  � ��$	 2,0q���  � ��$ 12(���  �@�Cursor$
 Hand����  � ��
 ���� ��  � ��8   ��  � ��$ 1q���  � ��
 ���� ��  � �� �� " 	�.R��� $ border��#��z��#��z��#��z��$ 4��._��� #��z��$��Center=�$��Center=�#��z�� P� 	 �@J�IsMouseOver$

 True��. ��� 8   � ���� $ border�� � �� �#EAF4FCP� 	 �@d�	IsChecked$

 TrueY�. ���  � ��
 ������� $ border��8   � ���� " ��.���  �@J�SnapsToDevicePixels$
 true����  � ��8   ��  � ��8  ��  � ��$ 5,0,0,0q���  � ��$ Center=���  � �� �� " ��.R�� .+��� $ 
background��#��z��#��z��$��1q�$ 3��$��18��$��18��F� $ 	checkMark��"���� 	=  z � z  � р�� @KL 	8�� $��2��$y�	Collapsed=� P� 	 �@d�	IsChecked$

 TrueY�. ��� $ 	checkMark�� � y�$ Visible=�P� 	 �@J�IsMouseOver$

 True��. ��� $ 
background��8   � ���� " ��8   .���  � ��
 �<�� P� 	 �@J�IsMouseOver$

 True��. ���  � �� �#B22222P� 	 � ��$

 True��. ���  � �� �#8B0000�� " ��8   .���  � ��
 �<����  � ��
 �<�� P� 	 �@J�IsMouseOver$

 True��. ���  � �� �#FFF0F5��  � ��
 �<��P� 	 � ��$

 True��. ���  � ��
 �<����  � ��
 ������� " ��.���  � ��
 ���� ��  � ��8   ��  � ��8  ��  � ��$ 1q���  � ��$
 12,6q���  �@�Cursor$
 Hand����  � �� �� " ��.R��� $ border��#��z��#��z��#��z��$ 15��._��� $ 
Horizontal=�$��Center=�#��z��.+��� #��z 8�� $��Center=�$��0,0,6,0q�$��14(���  P� 	 �@J�IsMouseOver$

 True��. ��� 8   � ���� $ border��8   � ��P� 	 � ��$

 True��. ��� $ border�� � �� �#EAF4FC
    �� " ��.���  � ��
 ���� ��  � ��8   ��  � ��8  ��  � ��$ 1q���  � ��$
 12,8q���  �@�Cursor$
 Hand����  � �� �� " ��.R��� $ border��#��z��#��z��#��z��$ 4��._��� $��Center=�$��Center=�#��z�� P� 	 �@J�IsMouseOver$

 True��. ��� 8   � ���� $ border��8   � ��P� 	 � {�$
 False��. ���  �@J�Opacity$	 0.5Q��� " ��.���  �@J�RenderTransformOrigin$ 0.5, 0.5&���  � z� �� ��  �@J�Effect   $ 1Q�$ 10Q�$ #E0E0E0��$	 0.6Q� P� 	 �@J�IsMouseOver$

 True�� �� .���� .��W� $9 3(UIElement.RenderTransform).(ScaleTransform.ScaleX)� 1.01$ 0:0:0.2D�W� $9 3(UIElement.RenderTransform).(ScaleTransform.ScaleY)� 1.01$ 0:0:0.2D�W� $6 0(UIElement.Effect).(DropShadowEffect.BlurRadius)� 20$ 0:0:0.2D�W� $7 1(UIElement.Effect).(DropShadowEffect.ShadowDepth)� 3$ 0:0:0.2D� �� .���� .��W� $9 3(UIElement.RenderTransform).(ScaleTransform.ScaleX)� 1.0$ 0:0:0.3D�W� $9 3(UIElement.RenderTransform).(ScaleTransform.ScaleY)� 1.0$ 0:0:0.3D�W� $6 0(UIElement.Effect).(DropShadowEffect.BlurRadius)� 10$ 0:0:0.3D�W� $7 1(UIElement.Effect).(DropShadowEffect.ShadowDepth)� 1$ 0:0:0.3D�