
#include <sstream>
#include <string>
#include <iostream>
#include <opencv2\highgui.hpp>
#include <vector>
#include "C:/opencv/build/include/opencv2/imgproc.hpp"
#include "Checker.h"

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
		circle(frame, Point(checkers.at(i).GetX() , checkers.at(i).GetY()), 10, Scalar(0, 0, 255));
		putText(frame, intToString(checkers.at(i).GetX()) + " , " + intToString(checkers.at(i).GetY()), cv::Point(checkers.at(i).GetX(), checkers.at(i).GetY() + 20), 1, 1, Scalar(0, 255, 0));
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

void drawLine(Point start, Point end,Mat& frame) {
	line(frame, start, end, Scalar(255, 255, 255), 8, 0);
	putText(frame, intToString(start.x)+" , "+ intToString(start.y), start, 1, 1, Scalar(255, 255, 0));
	putText(frame, intToString(end.x) + " , " + intToString(end.y), end, 1, 1, Scalar(255, 255, 0));
}

void drawLinesOnBoard(Point P1, Point P2, Mat& frame) {

	int middlex1 = (P2.x - P1.x) / 2;
	int middley1 = (P2.y - P1.y) / 2;

	int middlex2 = (P2.x - P1.x) / 4;
	int middley2 = (P2.y - P1.y) / 4;

	int middlex3 = (P2.x - P1.x) / 8;
	int middley3 = (P2.y - P1.y) / 8;

	//tier1

	Point P3 = Point(P1.x, P1.y + middley1);
	Point P4 = Point(P2.x, P2.y - middley1);
	Point P5 = Point(P1.x + middlex1, P1.y);
	Point P6 = Point(P2.x - middlex1, P2.y);

	 drawLine(P3, P4, frame);
	 drawLine(P5, P6, frame);

	 //tier2

	 Point P7 = Point(P1.x, P1.y + middley1 + middley2);
	 Point P8 = Point(P2.x, P2.y - middley2);
	 Point P9 = Point(P1.x + middlex1 + middlex2, P1.y);
	 Point P10 = Point(P2.x - middlex2, P2.y);

	 drawLine(P7, P8, frame);
	 drawLine(P9, P10, frame);

	 Point P71 = Point(P1.x, P1.y + middley2);
	 Point P81 = Point(P2.x, P2.y - middley1- middley2);
	 Point P91 = Point(P1.x + middlex2, P1.y);
	 Point P101 = Point(P2.x - middlex1- middlex2, P2.y);

	 drawLine(P71, P81, frame);
	 drawLine(P91, P101, frame);


	 //tier 3

	 Point P11 = Point(P1.x, P1.y + middley1 + middley2 + middley3);
	 Point P12 = Point(P2.x, P2.y - middley3);
	 Point P13 = Point(P1.x + middlex1 + middlex2 + middlex3, P1.y);
	 Point P14 = Point(P2.x - middlex3, P2.y);

	 drawLine(P11, P12, frame);
	 drawLine(P13, P14, frame);

	 Point P111 = Point(P1.x, P1.y +  middley1 + middley3);
	 Point P121 = Point(P2.x, P2.y -middley2- middley3);
	 Point P131 = Point(P1.x + middlex1 + middlex3, P1.y);
	 Point P141 = Point(P2.x - middlex2-middlex3, P2.y);

	 drawLine(P111, P121, frame);
	 drawLine(P131, P141, frame);

	 Point P112 = Point(P1.x, P1.y +  middley2 + middley3);
	 Point P122 = Point(P2.x, P2.y - middley1 - middley3);
	 Point P132 = Point(P1.x +  middlex2 + middlex3, P1.y);
	 Point P142 = Point(P2.x - middlex1 -middlex3, P2.y);

	 drawLine(P112, P122, frame);
	 drawLine(P132, P142, frame);

	 Point P113 = Point(P1.x, P1.y + middley3);
	 Point P123 = Point(P2.x, P2.y - middley1 - middley2 - middley3);
	 Point P133 = Point(P1.x +  middlex3, P1.y);
	 Point P143 = Point(P2.x - middlex1 - middlex2-middlex3, P2.y);

	 drawLine(P113, P123, frame);
	 drawLine(P133, P143, frame);
}

void drawBorder(vector <Checker> checkers, Mat& frame) {
	Point P1 = Point(checkers[0].GetX(), checkers[0].GetY());
	Point P2 = Point(checkers[1].GetX(), checkers[1].GetY());
	rectangle(frame, P1, P2, Scalar(0, 0, 255), 8, 0);
	drawLinesOnBoard(P1, P2, frame);
}



void trackFilteredObject(Mat threshold, Mat HSV, Mat & cameraFeed,string name) {

	vector <Checker> player1_checkers;
	vector <Checker> player2_checkers;
	vector <Checker> borders;

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

					if (checker.GetPlayerName() == "player1")
					{
						player1_checkers.push_back(checker);
					}

					if (checker.GetPlayerName() == "player2")
					{
						player2_checkers.push_back(checker);
					}

					if (checker.GetPlayerName() == "border")
					{
						borders.push_back(checker);
					}

					objectFound = true;

				}
				else objectFound = false;


			}
			//let user know you found an object
			if (objectFound == true) {
				//draw object location on screen
				drawObject(player1_checkers, cameraFeed);
				drawObject(player2_checkers, cameraFeed);
				drawObject(borders, cameraFeed);
				if (borders.size() == 2) drawBorder(borders,cameraFeed);
			}

		}
		else putText(cameraFeed, "TOO MUCH NOISE! ADJUST FILTER", Point(0, 50), 1, 2, Scalar(0, 0, 255), 2);
	}
}




int main(int argc, char* argv[])
{
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
	capture.open(0);
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

			Checker player1("Mr.White"), player2("Mr.Blue"), border("border");

			player1.SetHSVmin(Scalar(85,28,203));
			player1.SetHSVmax(Scalar(125,111, 256));

			player2.SetHSVmin(Scalar(99, 173, 100));
			player2.SetHSVmax(Scalar(148, 256, 205));

			border.SetHSVmin(Scalar(0, 0, 250));
			border.SetHSVmax(Scalar(2, 4, 256));

			cvtColor(cameraFeed, HSV, COLOR_BGR2HSV);
			medianBlur(HSV, HSV, 12);
			inRange(HSV, player1.GetHSVmin(), player1.GetHSVmax(), threshold);
			morphOps(threshold);
			trackFilteredObject(threshold, HSV, cameraFeed,"Mr.White");

			cvtColor(cameraFeed, HSV, COLOR_BGR2HSV);
			medianBlur(HSV, HSV, 12);
			inRange(HSV, player2.GetHSVmin(), player2.GetHSVmax(), threshold);
			morphOps(threshold);
			trackFilteredObject(threshold, HSV, cameraFeed, "Mr.Blue");

			cvtColor(cameraFeed, HSV, COLOR_BGR2HSV);
			medianBlur(HSV, HSV, 12);
			inRange(HSV, border.GetHSVmin(), border.GetHSVmax(), threshold);
			morphOps(threshold);
			trackFilteredObject(threshold, HSV, cameraFeed, "border");
		}

		imshow(windowName, cameraFeed);
		waitKey(30); // 30 ms delay
	}


	return 0;
}
