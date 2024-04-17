
from flask import Flask, request, render_template,url_for,redirect
app = Flask(__name__)
from PIL import  Image
from utils import reader,Train
from UserAttendance import UserAttendance
import cv2
readerFile=reader()
trainModels=Train()
user=UserAttendance()
import numpy as np
import json

# Open the JSON file for reading
with open(r'SessionInfo', 'r') as f:
    # Load the contents of the file into a dictionary
    data = json.load(f)

@app.route('/')
def home():
    try:
        return render_template('home.html')
    except:
        return render_template('error.html')

@app.route('/error')
def error():
    return render_template('error.html')
@app.route('/train')
def train():
    return render_template('train.html')
@app.route('/add')
def add():
    trainModels.trainUsingSvc()
    return render_template("end.html")
@app.route('/singup')
def singup():
    return render_template("regester.html")

@app.route('/start')
def start():
    Info=user.take_Attendance(data)
    print(Info)
    header,rows=readerFile.AadIntoTable()
    return render_template("home.html",header=header,rows=rows)
@app.route('/regester', methods=['GET', 'POST'])
def regester():
    if request.method == "POST":
        id = request.form['newuserid']
        image = request.files['upload']
        img = Image.open(image)
        img = np.array(img)
        img = Image.fromarray(img)
        img.save('image_gray.png')
        img=cv2.imread('image_gray.png')
        user.regestrion(img,id)
        print(" regestrion are done")
        trainModels.trainUsingSvc()
        print("successful process")
        return render_template('end.html')
    else:
        return render_template('error.html')

if __name__ == '__main__':
    app.run(debug=True)
