from flask import Flask, render_template, request, Response
import cv2
from datetime import date
from datetime import datetime
import face_recognition
import numpy as np
import pickle
app = Flask(__name__)
camera = cv2.VideoCapture(r'D:\New folder (5)\Video.mp4')
svc_model = pickle.load(open('model.pkl', 'rb'))
def gen_frames():
    while True:
        student_id = []
        info = []
        success, frame = camera.read()
        face_cascade = cv2.CascadeClassifier("models\haarcascade_frontalface_default.xml")
        gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
        small_frame = cv2.resize(frame, (0, 0), fx=0.25, fy=0.25)
        rgb_small_frame = small_frame[:, :, ::-1]
        faces = face_cascade.detectMultiScale(gray, 1.3, 5)
        face_locations = face_recognition.face_locations(rgb_small_frame)
        if face_locations != []:
            face_encodings = face_recognition.face_encodings(rgb_small_frame, face_locations)
            face_encodings = np.array(face_encodings)
            id = svc_model.predict(face_encodings)
            id = id[0]
            #now = datetime.now()
            #dt_string = now.strftime("%d/%m/%Y %H:%M:%S")
            #time = dt_string.split(" ")[1]
            # df = pd.read_csv(f'Attendance/Attendance-{datetoday}.csv')
            # print("your id id:", id[0])
            if int(id) not in student_id:
              student_id.append(int(id))
            for (x, y, w, h) in faces:
                cv2.rectangle(frame, (x, y), (x + w, y + h), (255, 0, 255), 1)
                cv2.putText(frame,'marked'+ id, (x, y - 10),
                            cv2.FONT_HERSHEY_SIMPLEX, 0.5, (255, 255, 0), 2)
    #frame=cv2.imread("static\img\backgrond.jpg")
        ret, buffer = cv2.imencode('.jpg', frame)
        frame = buffer.tobytes()
        yield (b'--frame\r\n'
               b'Content-Type: image/jpeg\r\n\r\n' + frame + b'\r\n')  # concat frame one by one and show result
@app.route('/')
def index():
    """Video streaming home page."""
    return render_template('index.html')
@app.route('/video_feed')
def video_feed():
    """Video streaming route. Put this in the src attribute of an img tag."""
    return Response(gen_frames(), mimetype='multipart/x-mixed-replace; boundary=frame')
if __name__ == '__main__':
    app.run(debug=True)