import matplotlib.pyplot as plt
import numpy as np
import csv
from matplotlib.ticker import MaxNLocator

Ns = [1, 3, 6, 12, 24, 48]
Qs = [0.02, 0.04, 0.06, 0.08, 0.1, 0.12, 0.14, 0.16, 0.18, 0.2, 0.22, 0.24, 0.26, 0.28, 0.3, 0.32, 0.34, 0.36, 0.38, 0.4, 0.42, 0.44, 0.46, 0.48, 0.5]       

# print(data)

for i in range(len(Ns)):
    res = np.loadtxt("res/Ex9aa" + str(Ns[i]), unpack='False')
    plt.plot(Qs, res[0], label="Nakamuto")
    plt.plot(Qs, res[1], label="Grunspan")
    plt.xlabel("q")
    plt.ylabel("P(n, q)")
    plt.xticks(Qs, rotation = 80)
    plt.title("N = " + str(Ns[i]))
    plt.legend(loc='upper right')
    plt.savefig("res/PlotEx9aa" + str(Ns[i]), bbox_inches="tight")
    plt.close()
