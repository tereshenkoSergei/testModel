   M S B A M L   `   `   `  ����U  QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf  5   	    	TestModel   TestModel.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     5   	     � Title$  
MainWindow��5   	   $
��1000��6   $
��1000��6*   .��5	      � .+�5   
   �� .@�5      �� $)��#Задание параметров��6   .��5      ]� ��. .+�5      �� $	��Top=�6!   $	��300��66   .+�5      ]� .+�5      �� $
��Left=�6$   $/��)Количество испытуемых��6:   $	��170��6Z   �� 5      $
��Left=�5   !    � Minimum$ 1Q�5   !    � Maximum$	 200Q�5   !   $	��200��5   !    � Value 5   !   �� *StudentAmount+5   !   �� 5      $
��Left=�6$   ��6:   �� *StudentAmount+5      ]� 5      .+�5      �� $
��Left=�6$   $)��#Количество заданий��6:   $	��170��6W   �� 5      $
��Left=�5   !   $ 1Q�5   !   $	 200Q�5    !   $	��200��5!   !    5   !   �� *
TaskAmount+5!   !   �� 5"      $
��Left=�6$   ��6:   �� *
TaskAmount+5#      ]� 5$      .+�5%      �� $
��Left=�6$   $/��)Минимальная сложность��6:   $	��170��6Z   �� 5&      $
��Left=�5(   !   $ 4Q�5*   !   $	��200��5+   !    5'   !   �� *MaxComplexity+ 5)   !   �� *MinComplexity+5+   !   �� 5,      $
��Left=�6$   ��6:   �� *MaxComplexity+5-      ]� 5.      .+�5/      �� $
��Left=�6$   $1��+Максимальная сложность��6:   $	��170��6[   �� 50      $
��Left=�52   !   $ -4Q�53   !   $ 4Q�54   !   $	��200��55   !    51   !   �� *MinComplexity+55   !   �� 56      $
��Left=�6$   ��6:   �� *MinComplexity+57      ]� 58      .+�59      �� $
��Left=�6$   $B��<Максимальный уровень подготовки��6:   $	��170��6d   �� 5:      $
��Left=�5<   !   $ 4Q�5>   !   $	��200��5?   !    5;   !   �� *
MaxLevel+ 5=   !   �� *
MinLevel+5?   !   �� 5@      $
��Left=�6$   ��6:   �� *
MaxLevel+5A      ]� 5B      .+�5C      �� $
��Left=�6$   $@��:Минимальный уровень подготовки��6:   $	��170��6c   �� 5D      $
��Left=�5F   !   $ -4Q�5G   !   $ 4Q�5H   !   $	��200��5I   !    5E   !   �� *
MinLevel+5I   !   �� 5J      $
��Left=�6$   ��6:   �� *
MinLevel+5K      �� 5L      $R��LСгенерировать штуки (текст потом поменяю)��6!   ��6U   �� *GenerateTransactsCommand+5M      5S      5V      �� 5W      $5��/Распределение испытуемых��6   .��5X      � .+�5Y      #   LiveCharts.Wpf.CartesianChart  &    LiveCharts.Wpf.Charts.Base.Chart   LegendLocation$
 Left=�6T      Series 6)   �� *StudentSeriesCollection+   AxisX 5[         LiveCharts.Wpf.Axis     Title$ Salesman��6'      Labels 68   �� *StudentLabels+	   AxisY	 5^        $ 	Sold Apps��6'   
   LabelFormatter
 69   �� *	Formatter+5`      5a      5b      �� 5c      $/��)Распределение заданий��6   .��5d      � .+�5e        $
 Left=�6Q    6)   �� *TaskSeriesCollection+ 5g        $ Salesman��6'    68   �� *
TaskLabels+	 5j        $ 	Sold Apps��6'   
 69   �� *	Formatter+5l      5m      5n      5o      5q      5r      