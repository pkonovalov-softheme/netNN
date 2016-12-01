import tensorflow as tf
import numpy as np
import sys
import csv

W1_hist = []
W2_hist = []

B1_hist = []
B2_hist = []

args = sys.argv[1:]

W1val = float(args[0])
B1val = float(args[1])
W2val = float(args[2])
B2val = float(args[3])

passCount = int(args[4])
init_val = float(args[5])
target_val = float(args[6])


x_data = [init_val]
y_data = target_val

W1 = tf.Variable([W1val])
b1 = tf.Variable([B1val])
y1 = tf.nn.relu(W1 * x_data + b1)

W2 = tf.Variable([W2val])
b2 = tf.Variable([B2val])
y2 = tf.nn.relu(W2 * y1 + b2)

loss = tf.reduce_mean(tf.square(y2 - y_data))
optimizer = tf.train.GradientDescentOptimizer(0.01)
train = optimizer.minimize(loss)

init = tf.initialize_all_variables()

sess = tf.Session()
sess.run(init)

print("Init out = " + str(sess.run(y2)))


for step in range(passCount):
    sess.run(train)

    curW1 = sess.run(W1)
    curW2 = sess.run(W2)

    W1_hist.append(curW1)
    W2_hist.append(curW2)

    B1_hist.append(sess.run(b1))
    B2_hist.append(sess.run(b2))


W1_dump = open("D:\\netNN\\dump\\W1_dump.csv", 'w')
W1_wr = csv.writer(W1_dump, quoting=csv.QUOTE_ALL)
W1_wr.writerow(W1_hist)

B1_dump = open("D:\\netNN\\dump\\B1_dump.csv", 'w')
B1_wr = csv.writer(B1_dump, quoting=csv.QUOTE_ALL)
B1_wr.writerow(B1_hist)

W2_dump = open("D:\\netNN\\dump\\W2_dump.csv", 'w')
W2_wr = csv.writer(W2_dump, quoting=csv.QUOTE_ALL)
W2_wr.writerow(W2_hist)

B2_dump = open("D:\\netNN\\dump\\B2_dump.csv", 'w')
B2_wr = csv.writer(B2_dump, quoting=csv.QUOTE_ALL)
B2_wr.writerow(B2_hist)


#rows = zip(W1_hist,B1_hist,W2_hist,B2_hist)

#dump = open("D:\\netNN\\dump\\dump.csv", 'w')
#dump_wr = csv.writer(dump, quoting=csv.QUOTE_ALL)
#dump_wr.writerow(rows)
