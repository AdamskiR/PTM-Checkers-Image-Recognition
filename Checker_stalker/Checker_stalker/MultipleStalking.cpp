
#include <sstream>
#include <string>
#include <iostream>
#include <opencv2\highgui.hpp>
#include <vector>
#include "C:/opencv/build/include/opencv2/imgproc.hpp"
#include "Checker.h"
#include <Windows.h>


using namespace cv;
using namespace std;



//initial min and max HSV filter values.
//these will be changed using trackbars
int H_MIN = 0;
int H_MAX = 256;
int S_MIN = 0;
int S_MAX = 256;
int V_MIN = 0;
int V_MAX = 256;
//default capture width and height
const int FRAME_WIDTH = 1280;
const int FRAME_HEIGHT = 720;
//max number of objects to be detected in frame
const int MAX_NUM_OBJECTS = 50;
//minimum and maximum object area
const int MIN_OBJECT_AREA = 20 * 20;
const int MAX_OBJECT_AREA = FRAME_HEIGHT * FRAME_WIDTH / 1.5;
//names that will appear at the top of each window
const string windowName = "Original Image";
const string windowName1 = "HSV Image";
const string windowName2 = "Thresholded Image";
const string windowName3 = "After Morphological Operations";
const string trackbarWindowName = "Trackbars";
bool p1Bigger = false;

struct Tile {
	Point minX;
	Point minY;
	Point maxX;
	Point maxY;
	string name;
	int number;
	Point middle;
};

vector <Tile> tiles;
vector <Checker> player1_checkers;
vector <Checker> player2_checkers;

void on_trackbar(int, void*)
{//This function gets called whenever a
	// trackbar position is changed
}

string intToString(int number) {
	std::stringstream ss;
	ss << number;
	return ss.str();
}

void createTrackbars() {
	//create window for trackbars
	namedWindow(trackbarWindowName, 0);
	//create memory to store trackbar name on window
	char TrackbarName[50];
	sprintf_s(TrackbarName, "H_MIN", H_MIN);
	sprintf_s(TrackbarName, "H_MAX", H_MAX);
	sprintf_s(TrackbarName, "S_MIN", S_MIN);
	sprintf_s(TrackbarName, "S_MAX", S_MAX);
	sprintf_s(TrackbarName, "V_MIN", V_MIN);
	sprintf_s(TrackbarName, "V_MAX", V_MAX);
	//create trackbars and insert them into window
	//3 parameters are: the address of the variable that is changing when the trackbar is moved(eg.H_LOW),
	//the max value the trackbar can move (eg. H_HIGH), 
	//and the function that is called whenever the trackbar is moved(eg. on_trackbar)
	//                                  ---->    ---->     ---->      
	createTrackbar("H_MIN", trackbarWindowName, &H_MIN, H_MAX, on_trackbar);
	createTrackbar("H_MAX", trackbarWindowName, &H_MAX, H_MAX, on_trackbar);
	createTrackbar("S_MIN", trackbarWindowName, &S_MIN, S_MAX, on_trackbar);
	createTrackbar("S_MAX", trackbarWindowName, &S_MAX, S_MAX, on_trackbar);
	createTrackbar("V_MIN", trackbarWindowName, &V_MIN, V_MAX, on_trackbar);
	createTrackbar("V_MAX", trackbarWindowName, &V_MAX, V_MAX, on_trackbar);
}

void drawObject(vector <Checker> checkers, Mat& frame) {
	for (int i = 0; i < checkers.size(); i++)
	{
		//circle(frame, Point(checkers.at(i).GetX() , checkers.at(i).GetY() ), 50, Scalar(0, 0, 255));
		rectangle(frame, Point(checkers.at(i).GetX() - 50, checkers.at(i).GetY() - 50) , Point(checkers.at(i).GetX() + 50, checkers.at(i).GetY() + 50) , Scalar(0, 0, 255), 1,1 ,0);
		putText(frame, intToString(checkers.at(i).GetX()) + " , " + intToString(checkers.at(i).GetY()), cv::Point(checkers.at(i).GetX(), checkers.at(i).GetY() + 20), 1,1, Scalar(0, 255, 0));
		putText(frame, checkers.at(i).GetPlayerName(), cv::Point(checkers.at(i).GetX(), checkers.at(i).GetY() + 40), 1, 1, Scalar(255, 255, 0));
	}
}
void morphOps(Mat & thresh) {
	//create structuring element that will be used to "dilate" and "erode" image.
	//the element chosen here is a 3px by 3px rectangle
	Mat erodeElement = getStructuringElement(MORPH_RECT, Size(3, 3));
	//dilate with larger element so make sure object is nicely visible
	Mat dilateElement = getStructuringElement(MORPH_RECT, Size(8, 8));

	erode(thresh, thresh, erodeElement);
	erode(thresh, thresh, erodeElement);

	dilate(thresh, thresh, dilateElement);
	dilate(thresh, thresh, dilateElement);
}




void trackFilteredObject(Mat threshold, Mat HSV, Mat & cameraFeed,string name) {

	vector <Checker> bordersRed;
	vector <Checker> bordersBlue;

	Mat temp;
	threshold.copyTo(temp);
	//these two vectors needed for output of findContours
	vector< vector<Point> > contours;
	vector<Vec4i> hierarchy;
	//find contours of filtered image using openCV findContours function
	findContours(temp, contours, hierarchy, RETR_CCOMP, CHAIN_APPROX_SIMPLE);
	//use moments method to find our filtered object
	double refArea = 0;
	bool objectFound = false;
	if (hierarchy.size() > 0) {
		int numObjects = hierarchy.size();
		//if number of objects greater than MAX_NUM_OBJECTS we have a noisy filter
		if (numObjects < MAX_NUM_OBJECTS) {
			for (int index = 0; index >= 0; index = hierarchy[index][0]) {

				Moments moment = moments((cv::Mat)contours[index]);
				double area = moment.m00;

				//if the area is less than 20 px by 20px then it is probably just noise
				//if the area is the same as the 3/2 of the image size, probably just a bad filter
				//we only want the object with the largest area so we safe a reference area each
				//iteration and compare it to the area in the next iteration.
				if (area > MIN_OBJECT_AREA) {

					Checker checker(name);

					checker.SetX(moment.m10 / area);
					checker.SetY(moment.m01 / area);

					if (checker.GetPlayerName() == "Mr.White")
					{
						player1_checkers.push_back(checker);
					}

					if (checker.GetPlayerName() == "Mr.Black")
					{
						player2_checkers.push_back(checker);
					}

					if (checker.GetPlayerName() == "TileRed")
					{
						bordersRed.push_back(checker);
					}

					if (checker.GetPlayerName() == "TileBlue")
					{
						bordersBlue.push_back(checker);
					}

					objectFound = true;

				}
				else objectFound = false;


			}
			//let user know you found an object
			if (objectFound == true) {
				drawObject(bordersRed, cameraFeed);
				drawObject(bordersBlue, cameraFeed);
				
			}

		}
		else putText(cameraFeed, "TOO MUCH NOISE! ADJUST FILTER", Point(0, 50), 1, 2, Scalar(0, 0, 255), 2);
	}
}


void CameraIdDetection() {
	Mat cameraFeed;
	Mat threshold;
	Mat HSV;
	string noDeviceFound = "No camera connected to the PC";
	VideoCapture capture;

	for (int i = 0; i <= 999; i++) {

		try {

			capture.open(i);
			capture.set(CAP_PROP_FRAME_WIDTH, FRAME_WIDTH);
			capture.set(CAP_PROP_FRAME_HEIGHT, FRAME_HEIGHT);
			capture.read(cameraFeed);
			cvtColor(cameraFeed, HSV, COLOR_BGR2HSV);
			if (capture.isOpened() == true) {
				cout << i << ": this number is ID of working camera" << endl;
				Sleep(60);
			}
		}
		catch (...)
		{
			if (i == 999) {
				if (!capture.isOpened() == true) {
					cout << endl << "----------------" << endl;
					cout << "Exception: " << noDeviceFound;
					cout << endl << "----------------" << endl;
				}
			}
		}
	}
}

void DrawCheckers() {

	char board[64];
	for (int i = 0; i < 64; i++)
	{
		board[i] = '*';
	}
	for (int i = 0; i < player1_checkers.size(); i++)
		if (player1_checkers[i].tileNumber != 0) board[player1_checkers[i].tileNumber] = 'W';

	for (int i = 0; i < player2_checkers.size(); i++)
		if (player2_checkers[i].tileNumber != 0) board[player2_checkers[i].tileNumber] = 'B';

	for (int i = 1; i <= 64; i++)
	{
		if (i % 8 != 0) {
			cout << board[i - 1];
		}
		else {
			cout << board[i - 1] << endl;
		}

	}

	cout << endl << "----------------" << endl;
}

void DrawBoard() {

}

int main(int argc, char* argv[])
{
	//use this if if you need to detect your camera
	//CameraIdDetection();

	//if we would like to calibrate our filter values, set to true.
	bool calibrationMode = false;

	//Matrix to store each frame of the webcam feed
	Mat cameraFeed;
	Mat threshold;
	Mat HSV;

	if (calibrationMode) {
		//create slider bars for HSV filtering
		createTrackbars();
	}
	//video capture object to acquire webcam feed
	VideoCapture capture;
	//open capture object at location zero (default location for webcam)
	capture.open(701);
	//set height and width of capture frame
	capture.set(CAP_PROP_FRAME_WIDTH, FRAME_WIDTH);
	capture.set(CAP_PROP_FRAME_HEIGHT, FRAME_HEIGHT);
	//start an infinite loop where webcam feed is copied to cameraFeed matrix
	//all of our operations will be performed within this loop


	while (1) {
		//store image to matrix
		capture.read(cameraFeed);
		//convert frame from BGR to HSV colorspace
		cvtColor(cameraFeed, HSV, COLOR_BGR2HSV);

		if (calibrationMode == true) {
			//if in calibration mode, we track objects based on the HSV slider values.
			cvtColor(cameraFeed, HSV, COLOR_BGR2HSV);
			inRange(HSV, Scalar(H_MIN, S_MIN, V_MIN), Scalar(H_MAX, S_MAX, V_MAX), threshold);
			morphOps(threshold);
			imshow(windowName2, threshold);
			trackFilteredObject(threshold, HSV, cameraFeed,"");
		}
		else {

			Checker player1("Mr.White"), player2("Mr.Black"), borderBlue("TileBlue"), borderRed("TileRed");

			player1.SetHSVmin(Scalar(0,0,255));
			player1.SetHSVmax(Scalar(0,0, 256));

			player2.SetHSVmin(Scalar(0, 0, 0));
			player2.SetHSVmax(Scalar(256, 256, 30));

			borderBlue.SetHSVmin(Scalar(104, 154, 90));
			borderBlue.SetHSVmax(Scalar(173, 256, 256));

			borderRed.SetHSVmin(Scalar(0, 138, 171));
			borderRed.SetHSVmax(Scalar(256, 237, 256));

			/*cvtColor(cameraFeed, HSV, COLOR_BGR2HSV);
			//medianBlur(HSV, HSV, 12);
			inRange(HSV, player1.GetHSVmin(), player1.GetHSVmax(), threshold);
			morphOps(threshold);
			trackFilteredObject(threshold, HSV, cameraFeed,"Mr.White");

			cvtColor(cameraFeed, HSV, COLOR_BGR2HSV);
			//medianBlur(HSV, HSV, 12);
			inRange(HSV, player2.GetHSVmin(), player2.GetHSVmax(), threshold);
			morphOps(threshold);
			trackFilteredObject(threshold, HSV, cameraFeed, "Mr.Black");*/

			cvtColor(cameraFeed, HSV, COLOR_BGR2HSV);
			//medianBlur(HSV, HSV, 12);
			inRange(HSV, borderBlue.GetHSVmin(), borderBlue.GetHSVmax(), threshold);
			morphOps(threshold);
			trackFilteredObject(threshold, HSV, cameraFeed, "TileBlue");

			cvtColor(cameraFeed, HSV, COLOR_BGR2HSV);
			//medianBlur(HSV, HSV, 12);
			inRange(HSV, borderRed.GetHSVmin(), borderRed.GetHSVmax(), threshold);
			morphOps(threshold);
			trackFilteredObject(threshold, HSV, cameraFeed, "TileRed");

			DrawBoard();
			//DrawCheckers();
			player1_checkers.clear();
			player2_checkers.clear();
		}

		imshow(windowName, cameraFeed);
		waitKey(30); // 30 ms delay

	}


	return 0;
}
