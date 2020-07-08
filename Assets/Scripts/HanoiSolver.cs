using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HanoiSolver : MonoBehaviour
{
    //hanio disks 
    public List<GameObject> disks;

    //the points to create the simulation transform for each disk
    public Transform sourceTowerPoint;
    public Transform tempTowerPoint;
    public Transform destinationTowerPoint;

    public List<GameObject> diskOrder;          // store every gameobject in types of disks to simulate the transcation
    public List<Vector3> diskspositionFrom;     // list which store the first position to disk that will move to
    public List<Vector3> diskspositionTo;       // list which store the second position to disk that will move to

    public float speed;                         // to manage the speed from the inspector to make it more testable

    int index = 0;                              // variable to control the index of the postions for each disk
    /// <summary>
    /// Call when start the application
    /// </summary>
    void Start()
    {
        HanioSlover(disks.Count, sourceTowerPoint.position, destinationTowerPoint.position, tempTowerPoint.position);
    }
    /// <summary>
    /// Hanio disk Simualtion "Recursion Algorithm"
    /// </summary>
    /// <param name="numberofdisks"> number of disks we want to apply hanio on it </param>
    /// <param name="sourcePoistion"> the source tower point position </param>
    /// <param name="destinationPosition"> the destination tower point poistion </param>
    /// <param name="tempPosition"> the temo tower point position </param>
    void HanioSlover(int numberofdisks, Vector3 sourcePoistion, Vector3 destinationPosition, Vector3 tempPosition)
    {
        if (numberofdisks > 0)
        {
            HanioSlover(numberofdisks - 1, sourcePoistion, tempPosition, destinationPosition);
            PoistionStoring(disks[numberofdisks - 1], sourcePoistion, destinationPosition);
            HanioSlover(numberofdisks - 1, tempPosition, destinationPosition, sourcePoistion);
        }
        StartCoroutine(MoveAllDisks());
    }
    /// <summary>
    /// Sotring the position for every disk movement and will be (2^n-1) step
    /// </summary>
    /// <param name="disk"> which disk will move in the order</param>
    /// <param name="from"> first position which the disk will move from </param>
    /// <param name="to"> second position whcih the disk will move to it </param>
    void PoistionStoring(GameObject disk, Vector3 from, Vector3 to)
    {
        diskOrder.Add(disk);
        diskspositionFrom.Add(from);
        diskspositionTo.Add(to);
    }
    /// <summary>
    /// Simulate all the disks movement to achieve the AI
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveAllDisks()
    {
        for (int i = 0; i < Mathf.Pow(2, disks.Count) - 1; i++)
        {
            yield return new WaitForSeconds(2f);
            StartCoroutine(SimulateMovement(diskOrder[i], diskspositionFrom[i], diskspositionTo[i]));
        }

    }

    /// <summary>
    /// Simulate the disk movement with the external script iTween to make an animation while moving the disks also
    /// </summary>
    /// <param name="diskToMove"> disk we want to simulate </param>
    /// <param name="from"> the first position which the disk will move from </param>
    /// <param name="to">the destination position which the disk will move to it </param>
    /// <returns></returns>
    IEnumerator SimulateMovement(GameObject diskToMove, Vector3 from, Vector3 to)
    {
        iTween.MoveTo(diskToMove, from, 2 / speed);
        yield return new WaitForSeconds(2 / speed);
        iTween.MoveTo(diskToMove, to, 2 / speed);
        yield return new WaitForSeconds(2 / speed);


        RaycastHit hit;
        if (Physics.Raycast(to, Vector3.down, out hit, 6))
        {
            iTween.MoveTo(diskToMove, hit.point + Vector3.up * .05f, 2 / speed);
            yield return new WaitForSeconds(2 / speed);
        }
    }
}
