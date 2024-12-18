﻿namespace NativeCharts;

public delegate Task AsyncAction<in T1>(T1 arg1);
public delegate Task AsyncAction<in T1, in T2>(T1 arg1, T2 arg2);
public delegate Task AsyncAction<in T1, in T2, in T3>(T1 arg1, T2 arg2, T3 arg3);