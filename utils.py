import  pickle
import  numpy as np
import  time
from sklearn import  svm
from datetime import date
from datetime import datetime
datetoday = date.today().strftime("%m_%d_%y")
import csv
class reader:
    '''
    this class contain tow function reedFiles and AadIntoTable
    * readFiles responsible for read known_encodings after process  of encodings And it return two lists
     1-known_face_encodings,
     2-known_face_ids
    and AadIntoTable responsible for set data witch came from csv file into table on html home
    '''
    def readFiles(self):
        known_face_encodings = pickle.load(open('strorge\known_encodings.pickle', 'rb'))
        known_face_ids = pickle.load(open('strorge\id_encodings.pickle', 'rb'))
        return known_face_encodings, known_face_ids
    def AadIntoTable(self):
        with open(f'Attendance\Attendance-{datetoday}.csv', newline='') as csvfile:
            reader = csv.reader(csvfile)
            header = next(reader)
            rows = list(reader)
        return header, rows
class Train:
    '''
    train class contain one method
    for train users images
    '''
    def trainUsingSvc(self):
        images, ids = reader.readFiles(None)
        images = np.array(images)
        ids = np.array(ids)
        clf = svm.SVC()
        clf.fit(images, ids)
        pickle.dump(clf, open('model.pkl', 'wb'))

