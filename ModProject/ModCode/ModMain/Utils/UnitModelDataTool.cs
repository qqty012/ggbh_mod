namespace qqty_Modifier
{
    public class UnitModelDataTool
    {
        public PortraitModelData data;
        public UnitModelDataTool(PortraitModelData data) {
            this.data = data;
        }

        public void SexChange(UnitSexType change) {
            var sex = data.sex;
            int dValue = 40000;
            UnitSexType sexType = (UnitSexType)sex;
            if (sexType != change) {
                System.Collections.Generic.List<int> GetIDs(int _sex) {
                    var ids = new System.Collections.Generic.List<int>();
                    foreach (var _t1 in g.conf.roleDress._allConfList)
                        if (_t1.sex == _sex && _t1.active == 1) ids.Add(_t1.id);
                    return ids;
                }
                if (sexType == UnitSexType.Man && change == UnitSexType.Woman) {
                    var ids = GetIDs(2);
                    data.sex = 2;
                    if (data.hair != 0) data.hair += dValue;
                    if (!ids.Contains(data.hair)) data.hair = 55001;
                    if (data.hairFront != 0) data.hairFront += dValue;
                    if (!ids.Contains(data.hairFront)) data.hairFront = 56001;
                    if (data.head != 0) data.head += dValue;
                    if (!ids.Contains(data.head)) data.head = 51001;
                    if (data.eyebrows != 0) data.eyebrows += dValue;
                    if (!ids.Contains(data.eyebrows)) data.eyebrows = 52001;
                    if (data.eyes != 0) data.eyes += dValue;
                    if (!ids.Contains(data.eyes)) data.eyes = 57001;
                    if (data.nose != 0) data.nose += dValue;
                    if (!ids.Contains(data.nose)) data.nose = 50001;
                    if (data.mouth != 0) data.mouth += dValue;
                    if (!ids.Contains(data.mouth)) data.mouth = 59001;
                    if (data.body != 0) data.body += dValue;
                    if (!ids.Contains(data.body)) data.body = 58001;
                    if (data.forehead != 0) data.forehead += dValue;
                    if (!ids.Contains(data.forehead)) data.forehead = 0;
                    if (data.faceFull != 0) data.faceFull += dValue;
                    if (!ids.Contains(data.faceFull)) data.faceFull = 0;
                    if (data.faceLeft != 0) data.faceLeft += dValue;
                    if (!ids.Contains(data.faceLeft)) data.faceLeft = 0;
                    if (data.faceRight != 0) data.faceRight += dValue;
                    if (!ids.Contains(data.faceRight)) data.faceRight = 0;
                }
                if (sexType == UnitSexType.Woman && change == UnitSexType.Man) {
                    var ids = GetIDs(1);
                    data.sex = 1;
                    if (data.hair != 0) data.hair -= dValue;
                    if (!ids.Contains(data.hair)) data.hair = 15001;
                    if (data.hairFront != 0) data.hairFront -= dValue;
                    if (!ids.Contains(data.hairFront)) data.hairFront = 16001;
                    if (data.head != 0) data.head -= dValue;
                    if (!ids.Contains(data.head)) data.head = 11001;
                    if (data.eyebrows != 0) data.eyebrows -= dValue;
                    if (!ids.Contains(data.eyebrows)) data.eyebrows = 12001;
                    if (data.eyes != 0) data.eyes -= dValue;
                    if (!ids.Contains(data.eyes)) data.eyes = 17001;
                    if (data.nose != 0) data.nose -= dValue;
                    if (!ids.Contains(data.nose)) data.nose = 10001;
                    if (data.mouth != 0) data.mouth -= dValue;
                    if (!ids.Contains(data.mouth)) data.mouth = 19001;
                    if (data.body != 0) data.body -= dValue;
                    if (!ids.Contains(data.body)) data.body = 18001;
                    if (data.forehead != 0) data.forehead -= dValue;
                    if (!ids.Contains(data.forehead)) data.forehead = 13001;
                    if (data.faceFull != 0) data.faceFull -= dValue;
                    if (!ids.Contains(data.faceFull)) data.faceFull = 0;
                    if (data.faceLeft != 0) data.faceLeft -= dValue;
                    if (!ids.Contains(data.faceLeft)) data.faceLeft = 0;
                    if (data.faceRight != 0) data.faceRight -= dValue;
                    if (!ids.Contains(data.faceRight)) data.faceRight = 0;
                }
            }
        }

        public void UpdateData(string dataValue) {
            var param = dataValue.Split('|');
            if (param.Length != 15) return;
            data.sex = int.Parse(param[0]);
            data.hat = int.Parse(param[1]);
            data.hair = int.Parse(param[2]);
            data.hairFront = int.Parse(param[3]);
            data.head = int.Parse(param[4]);
            data.eyebrows = int.Parse(param[5]);
            data.eyes = int.Parse(param[6]);
            data.nose = int.Parse(param[7]);
            data.mouth = int.Parse(param[8]);
            data.body = int.Parse(param[9]);
            data.back = int.Parse(param[10]);
            data.forehead = int.Parse(param[11]);
            data.faceFull = int.Parse(param[12]);
            data.faceLeft = int.Parse(param[13]);
            data.faceRight = int.Parse(param[14]);
        }

        public string ToModelString() {
            return $"{data.sex}|{data.hat}|{data.hair}|{data.hairFront}|{data.head}|{data.eyebrows}|{data.eyes}|{data.nose}|{data.mouth}|{data.body}|{data.back}|{data.forehead}|{data.faceFull}|{data.faceLeft}|{data.faceRight}";
        }
    }
}
