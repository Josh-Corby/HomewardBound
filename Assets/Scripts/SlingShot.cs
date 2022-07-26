using UnityEngine;
using TMPro;

public class SlingShot : GameBehaviour<SlingShot>
{
    //bullet 
    public GameObject[] bullets;
    [HideInInspector]
    public GameObject currentBullet;
    private int bulletValue = 0;
    //bullet force
    public float shootForce, upwardForce;

    //Gun stats
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;

    int bulletsLeft, bulletsShot;
    [HideInInspector]
    public int ammo;
    //Recoil
    //public Rigidbody playerRb;
    public float recoilForce;

    //bools
    bool shooting, readyToShoot, reloading;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;

    //Graphics
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;

    //bug fixing :D
    public bool allowInvoke = true;

    private void Awake()
    {
        //make sure magazine is full
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Start()
    {
        bulletValue = 0;
        CycleBullets(bulletValue);
        UpdateAmmo();
    }

    private void Update()
    {
        if (OM.outfit == Outfits.Slingshot)        
        {
            if(!UI.buildPanelStatus)
            {
                MyInput();

                //Set ammo display, if it exists :D
                if (ammunitionDisplay != null)
                    ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
            }
     
            if (IM.mouseScrollY < 0)
            {
                CycleBullets(1);

            }
            if (IM.mouseScrollY > 0)
            {
                CycleBullets(-1);
            }
        }
    }

    private void CycleBullets(int val)
    {
        bulletValue += val;
        if (bulletValue < 0) bulletValue = (bullets.Length - 1);
        if (bulletValue > (bullets.Length - 1)) bulletValue = 0;

        currentBullet = bullets[bulletValue];
        Debug.Log(currentBullet.name);
        UI.ChangeAmmoTypeText();
        
    }

    private void MyInput()
    {
        //Check if allowed to hold down button and take corresponding input
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        //Reloading 
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();
        //Reload automatically when trying to shoot without ammo
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();

        //Shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            //Set bullets shot to 0
            bulletsShot = 0;

            Shoot();
        }
    }

    private void Shoot()
    {
        if(ammo > 0)
        {
            readyToShoot = false;

            //Find the exact hit position using a raycast
            Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //Just a ray through the middle of your current view
            RaycastHit hit;

            //check if ray hits something
            Vector3 targetPoint;
            if (Physics.Raycast(ray, out hit))
                targetPoint = hit.point;
            else
                targetPoint = ray.GetPoint(75); //Just a point far away from the player

            //Calculate direction from attackPoint to targetPoint
            Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

            //Calculate spread
            float x = Random.Range(-spread, spread);
            float y = Random.Range(-spread, spread);

            //Calculate new direction with spread
            Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0); //Just add spread to last direction

            //Instantiate bullet/projectile
            GameObject currentBul = Instantiate(currentBullet, attackPoint.position, Quaternion.identity); //store instantiated bullet in currentBullet
                                                                                                       //Rotate bullet to shoot direction
            currentBul.transform.forward = directionWithSpread.normalized;

            //Add forces to bullet
            currentBul.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
            currentBul.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);

            //Instantiate muzzle flash, if you have one
            if (muzzleFlash != null)
                Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

            bulletsLeft--;
            bulletsShot++;

            //Invoke resetShot function (if not already invoked), with your timeBetweenShooting
            if (allowInvoke)
            {
                Invoke("ResetShot", timeBetweenShooting);
                allowInvoke = false;

                //Add recoil to player (should only be called once)
                //playerRb.AddForce(-directionWithSpread.normalized * recoilForce, ForceMode.Impulse);
            }

            //if more than one bulletsPerTap make sure to repeat shoot function
            if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
                Invoke("Shoot", timeBetweenShots);

            SubtractAmmo();
        }
    }
       
    private void ResetShot()
    {
        //Allow shooting and invoking again
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime); //Invoke ReloadFinished function with your reloadTime as delay
    }
    private void ReloadFinished()
    {
        //Fill magazine
        bulletsLeft = magazineSize;
        reloading = false;
    }

    private void UpdateAmmo()
    {
        ammo = GM.pebblesCollected;
    }
    private void SubtractAmmo()
    {
        GM.pebblesCollected -= 1;
        UpdateAmmo();
        UI.UpdatePebblesCollected();
    }
}
