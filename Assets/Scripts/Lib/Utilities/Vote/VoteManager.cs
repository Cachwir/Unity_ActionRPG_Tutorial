using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteManager
{
    static public IVoter Vote(List<IVoter> voters)
    {
        float totalWeight = GetTotalWeight(voters);
        float reachedWeight = 0;
        float targetWeight = Random.Range(0, totalWeight);

        if (voters.Count == 0)
        {
            throw new System.Exception("No voters given to VoteManager.Vote");
        }

        foreach (IVoter voter in voters)
        {
            float weight = voter.GetWeight();

            if (weight <= 0)
            {
                throw new System.Exception("We've got a voter with a non normal weight of " + weight);
            }

            reachedWeight += weight;

            if (reachedWeight + weight >= targetWeight)
            {
                return voter;
            }
        }

        return null;
    }

    static protected float GetTotalWeight(List<IVoter> voters)
    {
        float totalWeight = 0;

        foreach (IVoter voter in voters)
        {
            totalWeight += voter.GetWeight();
        }

        return totalWeight;
    }
}
