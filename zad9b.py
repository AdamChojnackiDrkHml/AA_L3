import matplotlib.pyplot as plt
import numpy as np
import csv
from matplotlib.ticker import MaxNLocator

Qs = [0.001, 0.01, 0.1]
Ns = list(range(1, 16))
# print(Ns)

for i in range(len(Qs)):
    res = np.loadtxt("res/Ex9ab" + str(i), unpack='False')
    plt.plot(Ns, res[0], label="Nakamuto")
    plt.plot(Ns, res[1], label="Grunspan")
    plt.xlabel("n")
    plt.ylabel("P(n,q)")
    plt.xticks(Ns, rotation = 80)
    plt.title("Q = " + str(Qs[i]))
    plt.legend(loc='upper right')
    plt.savefig("res/PlotEx9ab" + str(i), bbox_inches="tight")
    plt.close()
