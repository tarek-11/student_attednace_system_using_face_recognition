import time as t
import os
import  pandas as pd
import cv2
import json
import pickle
import datetime
import random
from datetime import date
from datetime import datetime
import face_recognition
from utils import  reader,Train
datetoday = date.today().strftime("%m_%d_%y")
#id_Session = random.randint(1, 9999)
#### If these directories don't exist, create them
if not os.path.isdir('Attendance'):
    os.makedirs('Attendance')
if f'Attendance-{datetoday}.csv' not in os.listdir('Attendance'):
    with open(f'Attendance/Attendance-{datetoday}.csv', 'w') as f:
        f.write('id,date,id_session')
if not os.path.isdir('Attendance'):
    os.makedirs('Attendance')
if f'Attendance-{datetoday}.csv' not in os.listdir('Attendance'):
    with open(f'Attendance/Attendance-{datetoday}.csv', 'w') as f:
        f.write('id,date,id_session')
class UserAttendance:
    '''
    This chapter contains a set of user functions
    * regestrion function,take Attendance
    '''
    #### Add Attendance of a specifi
    def regestrion(self,img, id):
        '''
            The registration function for a new student, and it is done by uploading a picture of the student attached to
            his identification number, and then the program extracts the important characteristics
            of this user and saves them in a file so that we can then carry out the training process
        '''
        # img = cv2.imread(img)
        img = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)
        img = face_recognition.face_encodings(img)[0]
        known_face_encodings, known_face_ids = reader.readFiles(None)
        known_face_encodings.append(img)
        known_face_ids.append(id)
        fil1e = open("strorge\known_encodings.pickle", 'wb')
        pickle.dump(known_face_encodings, fil1e)
        fil1e.close()
        fil12 = open("strorge\id_encodings.pickle", 'wb')
        pickle.dump(known_face_ids, fil12)
        fil12.close()
    ####################################################################################################################

    # Access the data in the dictionary
    def take_Attendance(self,data):
        student_id = []
        info = []
        session_id=data['session_id']
        '''
            The function of taking attendance is the student whispering in front of the camera and the camera recognizes him,
             and the time and identification number are saved in an external file
        '''

        svc_model = pickle.load(open('model.pkl', 'rb'))
        import numpy as np
        import cv2
        face_cascade = cv2.CascadeClassifier("models\haarcascade_frontalface_default.xml")
        import cv2
        # Open the default camera
        camera = cv2.VideoCapture(0)
        # Initialize counters
        c=data['time_of_session']
        while True:
            import time
            ret, frame = camera.read()
            gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
            small_frame = cv2.resize(frame, (0, 0), fx=0.25, fy=0.25)
            rgb_small_frame = small_frame[:, :, ::-1]
            faces = face_cascade.detectMultiScale(gray, 1.3, 5)
            mins, secs = divmod(c, 60)
            timer = '{:02d}:{:02d}'.format(mins, secs)
            time.sleep(1)
            print(timer)
            cv2.putText(frame, timer, (150, 30),
                        cv2.FONT_HERSHEY_SIMPLEX, 0.5, (255, 255, 0), 2)
            c-=1
            face_locations = face_recognition.face_locations(rgb_small_frame)
            if face_locations != []:
                face_encodings = face_recognition.face_encodings(rgb_small_frame, face_locations)
                face_encodings = np.array(face_encodings)
                id = svc_model.predict(face_encodings)
                id = id[0]
                now = datetime.now()
                dt_string = now.strftime("%d/%m/%Y %H:%M:%S")
                time = dt_string.split(" ")[1]
                #df = pd.read_csv(f'Attendance/Attendance-{datetoday}.csv')
                #print("your id id:", id[0])
                if int(id) not in student_id:
                    student_id.append(int(id))
                for (x, y, w, h) in faces:
                    #cv2.rectangle(frame, (x, y), (x + w, y + h), (255, 0, 255), 1)
                    cv2.putText(frame,  id+ 'marked', (x, y - 10),
                                cv2.FONT_HERSHEY_SIMPLEX, 0.5, (255, 255, 0), 2)
            key = cv2.waitKey(1)
            if key == ord('q') or c==0:
                break
            cv2.imshow('Camera', frame)
            # Release the camera and close any open windows
        camera.release()
        cv2.destroyAllWindows()
        info.insert(0, student_id)
        info.insert(1,session_id )
        information = dict(Student_id=info[0],
                    session_id=info[1])
        result = json.dumps(information)
        return result

