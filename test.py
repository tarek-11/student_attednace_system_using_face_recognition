from flask import Flask, Response, request,jsonify






# Load known face encodings and names from a text file or database
# ...
import pickle
def readFiles():
    known_face_encodings = pickle.load(open('strorge\known_encodings.pickle', 'rb'))
    known_face_ids = pickle.load(open('strorge\id_encodings.pickle', 'rb'))
    return known_face_encodings, known_face_ids

known_face_encodings,known_face_names = readFiles()
from flask import Flask, Response, request, jsonify
import cv2
import cv2
import face_recognition
from flask import Flask, jsonify

app = Flask(__name__)

# Load the trained model
known_faces = known_face_encodings
known_names = known_face_names


# Load the labeled images and train the model
# ...

@app.route('/recognize', methods=['POST'])
def recognize_faces():
    vid=request.files['image']
    # Capture live video
    video_capture = cv2.VideoCapture(vid)
    while True:
        # Grab a single frame of video
        ret, frame = video_capture.read()
        # Convert the image from BGR color (which OpenCV uses) to RGB color (which face_recognition uses)
        rgb_frame = frame[:, :, ::-1]
        # Find all the faces in the current frame of video
        face_locations = face_recognition.face_locations(rgb_frame)
        face_encodings = face_recognition.face_encodings(rgb_frame, face_locations)

        # Loop through each face in this frame of video
        for face_encoding in face_encodings:
            # See if the face is a match for the known faces
            matches = face_recognition.compare_faces(known_faces, face_encoding)
            name = "Unknown"

            # If we find a match, get the name of the person
            if True in matches:
                index = matches.index(True)
                name = known_names[index]

            # Return the name as a JSON object
            response = jsonify({"name": name})
            return response

        # Release the video capture object
        video_capture.release()


if __name__ == '__main__':
    app.run(debug=True)

if __name__ == '__main__':
    app.run()